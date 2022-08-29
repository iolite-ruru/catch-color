using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaggerColor : MonoBehaviour
{
    public static TaggerColor Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    private Image goggles;

    public void SetGogglesColor()
    {
        var tagger=FindObjectOfType<InGameTaggerMover>() ;
        Debug.Log(tagger);
        if (tagger != null) goggles.color = PlayerColor.GetColor(tagger.playerColor);
    }


}
