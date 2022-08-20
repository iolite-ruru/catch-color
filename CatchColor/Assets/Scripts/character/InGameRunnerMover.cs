using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            CmdSetPlayerCharacter(myRoomPlayer.playerColor); //���߿� �г��� �����Ҷ� �����ؾ���

            GameObject.Find("TextColor").GetComponent<Text>().text = myRoomPlayer.playerColor.ToString();
        }
    }

    [Command]
    private void CmdSetPlayerCharacter(MyColor color)
    {
        playerColor = color;
    }
}
