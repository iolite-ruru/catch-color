using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunnerCount : MonoBehaviour
{
    [SerializeField]
    private Text runnerCnt;
    public static RunnerCount Instance;

    private void Awake()
    {
        Instance = this;
    }

    

    public void SetRunnerCounter(string newStr)
    {
        runnerCnt.text = newStr;
    }
}
