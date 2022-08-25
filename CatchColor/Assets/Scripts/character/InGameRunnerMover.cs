using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameRunnerMover : CharacterMover
{
    public static int deadCount = 0;
    public static bool isEnd = false;

    [SyncVar(hook = nameof(SetPlayerState_Hook))]
    public State playerState;
    public void SetPlayerState_Hook(State _, State state)
    {
        playerState = state;
        deadCount++;
        if (hasAuthority)
        {
            isMovable = false;
            cam.transform.position = new Vector3(0f, 20f, -10f);
            cam.transform.rotation = Quaternion.Euler(50f,0f,0f);
        }
        if (deadCount == FindObjectsOfType<InGameRunnerMover>().Length)
        {
            Debug.Log("모든 도망자 잡음~ 술래가 이김");

            if (isServer)
            {
                deadCount = 0;
                isEnd = true;
            }

        }
    }



    [ClientRpc]
    public void RpcRendererFalse()
    {
        renderer.enabled = false;
    }


    public override void Start()
    {
        renderer = transform.Find("Body").GetComponent<SkinnedMeshRenderer>();
        renderer.material.color = PlayerColor.GetColor(playerColor);

        base.Start();

        if (hasAuthority)
        {
/*            cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 1f, 0f);*/

            cam = Camera.main;
            cam.transform.SetParent(transform.Find("Body").transform);
            cam.transform.localPosition = new Vector3(0f, 2.5f, -1.5f);

            playerState = State.Alive;
            
            var myRoomPlayer = RoomPlayer.MyRoomPlayer;
            CmdSetPlayerCharacter(myRoomPlayer.playerColor); //나중에 닉네임 설정할때 수정해야함

            GameObject.Find("TextColor").GetComponent<Text>().text = myRoomPlayer.playerColor.ToString();
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

}
