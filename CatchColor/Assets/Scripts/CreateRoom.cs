using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    [SerializeField]
    private InputField nicknameInputField;

    
    public void CreateRoomF()
    {
        if (nicknameInputField.text != "")
        {
            PlayerSettings.nickname = nicknameInputField.text;
            var manager = NetworkManager.singleton;
            manager.StartHost();
        }
        else
        {
            //nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }
       
    }

    public void EnterRoomF()
    {
        
        if (nicknameInputField.text != "")
        {
            PlayerSettings.nickname = nicknameInputField.text;
            var manager = NetworkManager.singleton;
            manager.StartClient();
        }
        else
        {
            //nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }
    }
}
