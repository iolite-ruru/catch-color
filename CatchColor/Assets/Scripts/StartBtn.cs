using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class StartBtn : MonoBehaviour
{




    public void OnClickStartButton()
    {
        var players = FindObjectsOfType<RoomPlayer>();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].readyToBegin = true;
        }
        var manager = NetworkManager.singleton as RoomManager;
        manager.ServerChangeScene(manager.GameplayScene);
    }
}
