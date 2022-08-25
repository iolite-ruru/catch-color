using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoomPlayer : NetworkRoomPlayer
{
    
    [SyncVar]
    public MyColor playerColor;

    private static RoomPlayer myRoomPlayer;
    public static RoomPlayer MyRoomPlayer
    {
        get {
            if (myRoomPlayer == null)
            {
                var players=FindObjectsOfType<RoomPlayer>();
                foreach(var player in players)
                {
                    if (player.hasAuthority)
                    {
                        myRoomPlayer = player;
                    }
                }
            }
            return myRoomPlayer; 
        }
    }


    public CharacterMover lobbyPlayerCharacter;

    public new void Start()
    {
        base.Start();
        if (isServer)
        {
            SpawnLobbyPlayerCharacter();
        }
        LobyUIManager.Instance.UpdatePlyerCount();
    }

    private void OnDestroy()
    {
        if (LobyUIManager.Instance != null)
        {
            LobyUIManager.Instance.UpdatePlyerCount();
        }
    }

    private void SpawnLobbyPlayerCharacter()
    {

        int idx = Random.Range(0, 3);
        MyColor color;
        if (idx == 0) color = MyColor.Red;
        else if (idx == 1) color = MyColor.Green;
        else color = MyColor.Blue;
        playerColor = color;
        //CharacterMover.isChangeColor = true;
        //Debug.Log("RoomPlayer.cs >> isChangeColor = true");

    }


}
