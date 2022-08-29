using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LobyUIManager : MonoBehaviour
{
    [SerializeField]
    private Text playerList;

    [SerializeField]
    private Text address;

    [SerializeField]
    private Button startButton;

    public static LobyUIManager Instance;

    private void Awake()
    {
        playerList.text = "";
        Instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        //string localIP = "Not available, please check your network seetings!";
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        address.text = host.AddressList[0].ToString();

        /*foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                address.text = ip.ToString();
                //localIP = ip.ToString();
            }

        }*/


    }

    [SerializeField]
    private Text playerCountText;

    public void UpdatePlyerCount()
    {
        var manager = NetworkManager.singleton as RoomManager;
        var players = FindObjectsOfType<RoomPlayer>();
        playerCountText.text = string.Format("{0}/{1}", players.Length, manager.maxConnections);

        playerList.text = "";
        if (RoomPlayer.MyRoomPlayer.isServer)
        {
            for(int i=1; i<=players.Length; i++)
            {
                playerList.text += "Player[" + i + "]\n";
            }
        }
        else
        {
            for (int i = 1; i <= players.Length; i++)
            {
                playerList.text += "Player[" + i + "]\n";
            }
        }
    }

    public void ActiveStartButton()
    {
        startButton.gameObject.SetActive(true);
    }
}
