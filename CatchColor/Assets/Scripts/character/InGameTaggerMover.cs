using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class InGameTaggerMover : CharacterMover
{
    private bool isAttack = false;
    private bool isSwing = false;

    public float range;
    public float damage;
    public float attackDelay;
    public float attackDelayA;
    public float attackDelayB;

    private RaycastHit hitInfo;

    GameObject target;

  

    public override void Start()
    {
        base.Start();

        if (hasAuthority)
        {
            cam = Camera.main;
            cam.transform.SetParent(transform.Find("Head").transform);
            cam.transform.localPosition = new Vector3(0f, 0.017f, -0.01f);

            var myRoomPlayer = RoomPlayer.MyRoomPlayer;
            CmdSetPlayerCharacter(myRoomPlayer.playerColor); //나중에 닉네임 설정할때 수정해야함. 처음 생성 될때 색상, 닉네임 설정

            GameObject.Find("TextColor").GetComponent<Text>().text = myRoomPlayer.playerColor.ToString();
        }
    }

    public override void Update()
    {
        base.Update();

        if (hasAuthority)
        {
            CameraRotation();
            CharacterRotation();
            TryAttack();
        }
    }

    
    public override void SetLayer(int layerIndex)
    {
        cam.cullingMask = ~(1 << layerIndex);
        Debug.Log("===child(Tagger)");
    }
    
    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDelayA);
        isSwing = true;

        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(attackDelay - attackDelayA - attackDelayB);

        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;

                Debug.Log("==충돌: " + hitInfo.transform.name);

                TellServerToDestroyObject(target);
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        bool check = Physics.Raycast(transform.position, transform.forward, out hitInfo, range);

        //도망자만 죽게 바꿔야 함
        if (check&& hitInfo.transform.name == "Game Player Runagate")
        {
            target = hitInfo.transform.gameObject;
            return true;
        }
        return false;
    }

    [Client]
    public void TellServerToDestroyObject(GameObject obj)
    {
        CmdKillRunner(obj);
    }

    [Command]
    public void CmdKillRunner(GameObject obj)
    {
        //객체 삭제가 아니라 관전 상태(투명)가 되게 바꿔야함
        NetworkServer.Destroy(obj);
        //target.playerState = State.Dead;
        //target.RpcRendererFalse();
    }
}
