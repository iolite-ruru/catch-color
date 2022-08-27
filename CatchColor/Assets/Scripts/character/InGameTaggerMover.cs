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

    InGameRunnerMover target;


    public override void SetLayerIndex_Hook(int oldLayer, int newLayer)
    {
        //Debug.Log("Tagger >> Test >> cam : " + newLayer);
        cam.cullingMask = ~(1 << newLayer);
        //isChangeColor = false;
    }


    public override void Start()
    {
        renderer = transform.Find("Body").Find("Glasses").GetComponent<MeshRenderer>();
        renderer.material.color = PlayerColor.GetColor(playerColor);

        transform.gameObject.layer = 6; // 술래: Player 레이어로 설정

        base.Start();

        if (hasAuthority)
        {
            //cam = Camera.main;
            //cam.transform.SetParent(transform.Find("Body").transform);
            //cam.transform.localPosition = new Vector3(0f, 2.5f, -1.5f);
            cam.transform.localPosition = new Vector3(0f, 2.5f, -1.5f);
            cam.transform.rotation = Quaternion.Euler(50f, 0f, 0f);

            var myRoomPlayer = RoomPlayer.MyRoomPlayer;
            CmdSetPlayerCharacter(myRoomPlayer.playerColor); //나중에 닉네임 설정할때 수정해야함. 처음 생성 될때 색상, 닉네임 설정

            GameObject.Find("TextColor").GetComponent<Text>().text = myRoomPlayer.playerColor.ToString();
            SetLayer(PlayerColor.GetColorInt(playerColor)+7);
        }
    }

    public override void Update()
    {
        base.Update();

        if (isMovable && hasAuthority)
        {
            CameraRotation();
            CharacterRotation();
            TryAttack();
            //cam.cullingMask = ~(1 << layer);
            //isChangeColor = false;
            /*if (isChangeColor)
            {
                cam.cullingMask = ~(1 << layer);
                isChangeColor = false;
                Debug.Log("Tagger >> Update >> isChangeColor");
            }*/
        }
    }

    public override void SetLayer(int layerIndex)
    {
        //cam.cullingMask = ~(1 << layerIndex);
        Debug.Log("Tagger >> SetLayer: "+layerIndex);
        layer = layerIndex;
        //isChangeColor = true;
        //Debug.Log("===child(Tagger): " + layerIndex + " => " + cam.cullingMask);
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

        //살아있는 도망자만
        if (check&& hitInfo.transform.name == "Cat_Runner(Clone)" && hitInfo.transform.gameObject.GetComponent<InGameRunnerMover>().playerState==State.Alive)
        {
            target = hitInfo.transform.gameObject.GetComponent<InGameRunnerMover>();
            return true;
        }
        return false;
    }

    [Client]
    public void TellServerToDestroyObject(InGameRunnerMover obj)
    {
        CmdKillRunner(obj);
    }

    [Command]
    public void CmdKillRunner(InGameRunnerMover obj)
    {
        obj.playerState = State.Dead;
        obj.RpcRendererFalse();
    }
}
