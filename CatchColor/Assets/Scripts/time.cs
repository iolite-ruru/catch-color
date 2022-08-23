using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class time : MonoBehaviour
{
    public float LimitTime=180;
    public Text txt_Time;
    public GameObject backImg;
    public GameObject endTxt;
    public float waitTime = 10;
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
        if (LimitTime > 0) { 
            LimitTime -= Time.deltaTime;
            if (Mathf.Round(LimitTime) > 60)
                txt_Time.text = "남은 시간 : " + (int)(Mathf.Round(LimitTime)) / 60 + "분 " + (Mathf.Round(LimitTime)) % 60 + "초";
            else txt_Time.text = "남은 시간 : " + Mathf.Round(LimitTime) + "초";
        }
        else
        {
            txt_Time.text = "";
            backImg.SetActive(true);
            endTxt.SetActive(true);
            NextTxt.SetActive(true);

            NextTxt2.text = "자동으로  " + Mathf.Round(waitTime) + "초 뒤 화면 전환됩니다.";
            waitTime -= Time.deltaTime;
            if(waitTime <= 1)
            {
                SceneManager.LoadScene("WaitingRoomScene");
            }
        }    
    }
}
