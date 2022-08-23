using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameRunnerMover : CharacterMover
{


    [SyncVar(hook = nameof(SetPlayerState_Hook))]
    public State playerState;
    public void SetPlayerState_Hook(State _, State state)
    {
        playerState = state;
        
    }

    [ClientRpc]
    public void RpcRendererFalse()
    {
        renderer.enabled = false;
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
            CmdSetPlayerCharacter(myRoomPlayer.playerColor); //나중에 닉네임 설정할때 수정해야함

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
        }
    }

}
