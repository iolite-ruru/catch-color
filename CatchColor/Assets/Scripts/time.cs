using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class time : NetworkBehaviour
{
    private AudioSource audio;
    public AudioClip sound;


        [SyncVar(hook = nameof(SetLimitTime_Hook))]
    public float LimitTime = 180;
    public void SetLimitTime_Hook(float _, float time)
    {
        LimitTime = time;
        if (Mathf.Round(LimitTime) > 60)
            txt_Time.text = "���� �ð� : " + (int)(Mathf.Round(LimitTime)) / 60 + "�� " + (Mathf.Round(LimitTime)) % 60 + "��";
        else txt_Time.text = "���� �ð� : " + Mathf.Round(LimitTime) + "��";
        if(LimitTime<0)
            this.audio.Play();
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
        NextTxt2.text = "�ڵ�����  " + Mathf.Round(waitTime) + "�� �� ȭ�� ��ȯ�˴ϴ�.";
    }
    public GameObject NextTxt;
    public Text NextTxt2;

    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.clip = this.sound;


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
            if (hasAuthority&&InGameRunnerMover.isEnd) CmdSetText(endTxt); //////////////ȣ�� �ȵ�
            RpcSetActive();

            waitTime -= Time.deltaTime;

        }
    }

    [Command]
    public void CmdSetText(GameObject text)
    {
        Debug.Log("���� �ٲ�");
        text.GetComponent<Text>().text = "���� �¸�";
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
