using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        /*Application.targetFrameRate = 60;
        OnDemandRendering.renderFrameInterval= 1;*/
        int setWidth = 1920;
        int setHeight = 1080;
        Screen.SetResolution(setWidth, setHeight, true);  //true:풀스크린, false:창
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined; // 게임 창 밖으로 마우스가 안나감
        //Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, true);
    }

}
