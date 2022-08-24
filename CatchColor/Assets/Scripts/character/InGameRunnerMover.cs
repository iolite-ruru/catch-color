using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameRunnerMover : CharacterMover
{
    public static int deadCount = 0;

    [SerializeField]
    protected SkinnedMeshRenderer[] myMesh = new SkinnedMeshRenderer[2];

    [SyncVar(hook = nameof(SetPlayerState_Hook))]
    public State playerState;
    public void SetPlayerState_Hook(State _, State state)
    {
        playerState = state;
        deadCount++;
        if (hasAuthority)
        {
            isMovable = false;
            cam = Camera.main;
            cam.transform.SetParent(transform.Find("Head").transform);
            cam.transform.localPosition = new Vector3(0f, 0.017f, -0.01f);
            /*cam.transform.position = new Vector3(0f, 20f, -10f);
            cam.transform.rotation = Quaternion.Euler(50f,0f,0f);*/
        }
        if(deadCount== FindObjectsOfType<InGameRunnerMover>().Length)
        {
            Debug.Log("��� ������ ����~ ������ �̱�");
            if (isServer)
            {
                //NetworkManager.singleton.StopHost();
            }

        } 
    }

    public override void SetLayer(int layerIndex)
    {
        gameObject.layer = layerIndex;
    }


    [ClientRpc]
    public void RpcRendererFalse()
    {
        //renderer.enabled = false;
    }


    public override void Start()
    {
        base.Start();

        if (hasAuthority)
        {
            cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 1f, 0f);

            playerState = State.Alive;
            
            var myRoomPlayer = RoomPlayer.MyRoomPlayer;
            CmdSetPlayerCharacter(myRoomPlayer.playerColor); //���߿� �г��� �����Ҷ� �����ؾ���

            GameObject.Find("TextColor").GetComponent<Text>().text = myRoomPlayer.playerColor.ToString();

            myMesh[0] = transform.Find("Head").GetComponent<SkinnedMeshRenderer>();
            myMesh[1] = transform.Find("Body").GetComponent<SkinnedMeshRenderer>(); //������Ʈ ���� ���� ���� ��
        }
    }

    public override void Update()
    {
        base.Update();

        if (isMovable && hasAuthority)
        {
            CameraRotation();
            CharacterRotation();
        }
    }

    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public MyColor playerColor;
    public override void SetPlayerColor_Hook(MyColor oldColor, MyColor newColor)
    {
        if (renderer == null)
        {
            renderer = gameObject.GetComponent<Renderer>();
        }
        test(newColor);
        //renderer.material.color = PlayerColor.GetColor(newColor); //������ ��� �� �ٲٱ�
    }
    
    public void test(MyColor newColor)
    {
        myMesh[0].material.color = PlayerColor.GetColor(newColor);
        myMesh[1].material.color = PlayerColor.GetColor(newColor);
    }

    [Command]
    public override void CmdSetColor(MyColor color, int idx)
    {
        playerColor = color;
        SetLayer(idx);
    }

    //���� ����
    [Command]
    protected override void CmdSetPlayerCharacter(MyColor color)
    {
        playerColor = color;
    }
}
