using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManager : NetworkRoomManager
{
    public int minPlyaerCount;
    public int TaggerCount;

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        Debug.Log($"NetworkRoomManager OnServerReady {conn}");
        //base.OnServerReady(conn);
        NetworkServer.SetClientReady(conn);

        if (conn != null && conn.identity != null)
        {
            GameObject roomPlayer = conn.identity.gameObject;

            // if null or not a room player, don't replace it
            if (roomPlayer != null && roomPlayer.GetComponent<NetworkRoomPlayer>() != null)
                SceneLoadedForPlayer(conn, roomPlayer);
        }
    }

    void SceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        Debug.Log($"�̰� ȣ��ǳ�~~~");

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
            // get start position from base class

            //���� ��¼���ؼ� ������ �־���ҵ�

            Transform startPos = GetStartPosition();
            gamePlayer = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        if (!OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer))
            return;

        // replace room player with game player
        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);
    }
}