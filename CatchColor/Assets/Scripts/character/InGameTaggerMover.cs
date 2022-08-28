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

    [SerializeField]
    private TrailRenderer attackTrail;

    [SerializeField]
    private ParticleSystem attackParticle;

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

        transform.gameObject.layer = 6; // ����: Player ���̾�� ����

        base.Start();

        if (hasAuthority)
        {
            cam.transform.localPosition = new Vector3(0f, 2.5f, -1.5f);
            cam.transform.rotation = Quaternion.Euler(50f, 0f, 0f);

            var myRoomPlayer = RoomPlayer.MyRoomPlayer;
            CmdSetPlayerCharacter(myRoomPlayer.playerColor); //���߿� �г��� �����Ҷ� �����ؾ���. ó�� ���� �ɶ� ����, �г��� ����

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
        }
    }

    public override void SetLayer(int layerIndex)
    {
        Debug.Log("Tagger >> SetLayer: "+layerIndex);
        layer = layerIndex;
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
        attackTrail.enabled = true;

        yield return new WaitForSeconds(attackDelayA);
        isSwing = true;

        // ���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(attackDelay - attackDelayA - attackDelayB);

        isAttack = false;
        attackTrail.enabled = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            //attackParticle.Play(); //�׽�Ʈ��
            if (CheckObject())
            {
                isSwing = false;

                attackParticle.Play();

                Debug.Log("==�浹: " + hitInfo.transform.name);

                TellServerToDestroyObject(target);
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        bool check = Physics.Raycast(transform.position, transform.forward, out hitInfo, range);

        //����ִ� �����ڸ�
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
        obj.destroyParticle.Play();
        obj.playerState = State.Dead;
        obj.RpcRendererFalse();
    }
}
