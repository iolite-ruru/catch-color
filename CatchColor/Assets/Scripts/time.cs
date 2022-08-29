using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class time : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetLimitTime_Hook))]
    public float LimitTime = 180;
    public void SetLimitTime_Hook(float _, float time)
    {
        LimitTime = time;
        string time_minute = "0" + (int)(Mathf.Round(LimitTime)) / 60;
        string time_second;
        if ((int)(Mathf.Round(LimitTime)) % 60 - 10 < 0)
            time_second = "0" + (int)(Mathf.Round(LimitTime)) % 60;
        else
            time_second = (int)(Mathf.Round(LimitTime)) % 60 +"";


        txt_Time.text = time_minute + " : " + time_second;
        //else txt_Time.text = "남은 시간 : " + Mathf.Round(LimitTime) + "초";
    }
    public Text txt_Time;
    public GameObject backImg;
    public GameObject endTxt;
    [SyncVar(hook = nameof(SetWaitTime_Hook))]
    public float waitTime = 10;
    public void SetWaitTime_Hook(float _, float time)
    {

        waitTime = time;
        if (waitTime < 0)
        {
            if (isServer)
            {
                NetworkManager.singleton.StopHost();
            }
        }
        NextTxt2.text = "자동으로  <color=red>" + Mathf.Round(waitTime) + "초</color> 뒤 타이틀 화면으로 전환됩니다.";
    }
    public GameObject NextTxt;
    public Text NextTxt2;

    void Start()
    {
        backImg.SetActive(false);
        endTxt.SetActive(false);
        NextTxt.SetActive(false);
    }
    void Update()
    {
        if (LimitTime > 0&&!InGameRunnerMover.isEnd)
        {
            LimitTime -= Time.deltaTime;

        }
        else
        {
            RpcSetActive();

            waitTime -= Time.deltaTime;

        }
    }
    [ClientRpc]
    public void RpcSetActive()
    {
       
        txt_Time.text = "";
        backImg.SetActive(true);
        endTxt.SetActive(true);
        NextTxt.SetActive(true);
    }


}
