using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    [SerializeField]
    private GameObject mainUI;
    [SerializeField]
    private GameObject clientUI;

    [SerializeField]
    private InputField IpInputField;

    //ȣ��Ʈ�� �� ����
    public void CreateRoomF()
    {
        var manager = RoomManager.singleton;
        manager.StartHost();

    }

    //Ŭ���̾�Ʈ UI ����
    public void OpenClientUI()
    {
        mainUI.SetActive(false);
        clientUI.SetActive(true);
    }

    //Ŭ���̾�Ʈ�� ����
    public void EnterRoomF()
    {
       

        Debug.Log(IpInputField.text);
        var manager = RoomManager.singleton;
        manager.networkAddress = IpInputField.text;
        manager.StartClient();

    }
}