using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRunnerMover : CharacterMover
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if (hasAuthority)
        {
            cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 1f, 0f);

            var myRoomPlayer = RoomPlayer.MyRoomPlayer;

            CmdSetPlayerCharacter(myRoomPlayer.playerColor); //나중에 닉네임 설정할때 수정해야함
        }
    }

    [Command]
    //https://youtu.be/OweqlUihHP0?list=PLYQHfkihy4Aw6QjsZqwwbD4ihpwvm7N0U&t=439
}
