using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class time : MonoBehaviour
{
    public float LimitTime=180;
    public Text txt_Time;

    void Update()
    {
        LimitTime -= Time.deltaTime;
        if (LimitTime <=0)
        {
            txt_Time.text = "��";
            SceneManager.LoadScene("testSecen");
        }
        if (Mathf.Round(LimitTime) > 60)
                txt_Time.text = "���� �ð� : " + (int)(Mathf.Round(LimitTime))/60+"�� "+ (Mathf.Round(LimitTime) )% 60+"��";
            else txt_Time.text = "���� �ð� : " + Mathf.Round(LimitTime)+"��";
        
            
    }
}
