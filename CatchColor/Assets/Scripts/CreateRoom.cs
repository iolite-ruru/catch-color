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

    //호스트로 방 열기
    public void CreateRoomF()
    {
        var manager = RoomManager.singleton;
        manager.StartHost();

    }

    //클라이언트 UI 열기
    public void OpenClientUI()
    {
        mainUI.SetActive(false);
        clientUI.SetActive(true);
    }

    //클라이언트로 들어가기
    public void EnterRoomF()
    {
       

        Debug.Log(IpInputField.text);
        var manager = RoomManager.singleton;
        manager.networkAddress = IpInputField.text;
        manager.StartClient();

    }
}