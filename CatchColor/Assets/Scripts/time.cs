using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class time : NetworkBehaviour
{
    [SyncVar]
    public float LimitTime=180;
    public Text txt_Time;
    public GameObject backImg;
    public GameObject endTxt;
    [SyncVar]
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
        if (LimitTime > 1) { 
            LimitTime -= Time.deltaTime;
            if (Mathf.Round(LimitTime) > 60)
                txt_Time.text = "���� �ð� : " + (int)(Mathf.Round(LimitTime)) / 60 + "�� " + (Mathf.Round(LimitTime)) % 60 + "��";
            else txt_Time.text = "���� �ð� : " + Mathf.Round(LimitTime) + "��";
        }
        else
        {
            txt_Time.text = "";
            backImg.SetActive(true);
            endTxt.SetActive(true);
            NextTxt.SetActive(true);

            NextTxt2.text = "�ڵ�����  " + Mathf.Round(waitTime) + "�� �� ȭ�� ��ȯ�˴ϴ�.";
            waitTime -= Time.deltaTime;
            if(waitTime <= 1)
            {
                Debug.Log("��� ��ȯ!!!");
                if (isServer)
                {
                    NetworkManager.singleton.StopHost();
                }
            }
        }    
    }

    


    
}
