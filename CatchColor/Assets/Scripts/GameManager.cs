using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        int setWidth = 1920;
        int setHeight = 1080;
        Screen.SetResolution(setWidth, setHeight, true);  //true:풀스크린, false:창
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, true);
    }

}
