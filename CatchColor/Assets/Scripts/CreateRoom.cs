using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CreateRoom : MonoBehaviour
{
    public void CreateRoomF()
    {
        var manager = NetworkManager.singleton;
        manager.StartHost();
    }
}
