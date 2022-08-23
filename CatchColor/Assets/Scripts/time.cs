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
            txt_Time.text = "끝";
            SceneManager.LoadScene("testSecen");
        }
        if (Mathf.Round(LimitTime) > 60)
                txt_Time.text = "남은 시간 : " + (int)(Mathf.Round(LimitTime))/60+"분 "+ (Mathf.Round(LimitTime) )% 60+"초";
            else txt_Time.text = "남은 시간 : " + Mathf.Round(LimitTime)+"초";
        
            
    }
}
