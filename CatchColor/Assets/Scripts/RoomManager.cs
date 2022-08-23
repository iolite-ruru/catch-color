using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoomManager : NetworkRoomManager {

    static int taggernum;

    public override void Start()
    {
        base.Start();
        taggernum = Random.Range(0, FindObjectsOfType<RoomPlayer>().Length); //이거 여기다가 하면 안됨~
    }

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        Debug.Log("플레이어 생성");
        //base.OnServerReady(conn);
        NetworkServer.SetClientReady(conn);

        if (conn != null && conn.identity != null )
        {
            GameObject roomPlayer = conn.identity.gameObject;

            // if null or not a room player, don't replace it
            if (roomPlayer != null && roomPlayer.GetComponent<NetworkRoomPlayer>() != null )
                SceneLoadedForPlayer(conn, roomPlayer);
        }
    }

    void SceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {

        if (IsSceneActive(RoomScene))
        {
            // cant be ready in room, add to ready list
            PendingPlayer pending;
            pending.conn = conn;
            pending.roomPlayer = roomPlayer;
            pendingPlayers.Add(pending);
            return;
        }

        GameObject gamePlayer = OnRoomServerCreateGamePlayer(conn, roomPlayer);
        if (gamePlayer == null)
        {
            
            Transform startPos = GetStartPosition();

            Debug.Log("술래 : " + taggernum);
            Debug.Log(RoomPlayer.MyRoomPlayer.playerColor+" index : " + RoomPlayer.MyRoomPlayer.index);
            if (taggernum == roomPlayer.GetComponent<RoomPlayer>().index)
            {
                gamePlayer =  Instantiate(spawnPrefabs[1], new Vector3(0,1,0), Quaternion.identity); //술래 생성
            }
            else
            {
                gamePlayer = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity); //도망자 생성
            }
            
            //gamePlayer = Instantiate(spawnPrefabs[1], new Vector3(0, 1, 0), Quaternion.identity); //술래 생성
        }

        if (!OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer))
            return;

        // replace room player with game player
        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);
    }

}