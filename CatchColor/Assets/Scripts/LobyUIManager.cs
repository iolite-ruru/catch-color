using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LobyUIManager : MonoBehaviour
{
    public static LobyUIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    private Text playerCountText;

    public void UpdatePlyerCount()
    {
        var manger = NetworkManager.singleton as RoomManager;
        var players = FindObjectsOfType<RoomPlayer>();
        playerCountText.text = string.Format("{0}/{1}", players.Length, manger.maxConnections);
    } 
}
