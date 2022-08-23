using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{

    public void CreateRoomF()
    {
        var manager = RoomManager.singleton;
        manager.StartHost();

    }

    public void EnterRoomF()
    {

        var manager = RoomManager.singleton;
        manager.StartClient();

    }
}
