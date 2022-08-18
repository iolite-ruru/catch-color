using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoomPlayer : NetworkRoomPlayer
{
    [SyncVar]
    public MyColor playerColor;

    [SyncVar]
    public string nickname;

    public CharacterMover lobbyPlayerCharacter;

    public void Start()
    {
        base.Start();
        if (isServer)
        {
            SpawnLobbyPlayerCharacter();
        }
        if (isLocalPlayer)
        {
            //CmdSetNickname(PlayerSettings.nickname);
        }
    }

    private void SpawnLobbyPlayerCharacter()
    {

        int idx = Random.Range(0, 3);
        MyColor color= MyColor.Red;
        if (idx == 0) color = MyColor.Red;
        else if (idx == 1) color = MyColor.Green;
        else if (idx == 2) color = MyColor.Blue;
        playerColor = color;

        var playerCharacter = Instantiate(NetworkManager.singleton.spawnPrefabs[0]).GetComponent<CharacterMover>();
        NetworkServer.Spawn(playerCharacter.gameObject, connectionToClient);
        playerCharacter.playerColor = color;
    }

    [Command]
    public void CmdSetNickname(string nick)
    {
        nickname = nick;
        lobbyPlayerCharacter.nickname = nickname;
    }
}
