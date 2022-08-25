using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 색상
public enum MyColor
{
    Red,
    Green,
    Blue,
}

public class PlayerColor
{
    private static List<Color> colors = new List<Color>()
    {
        Color.red,
        Color.green,
        Color.blue,
    };

    public static Color GetColor(MyColor color) { return colors[(int)color]; }
    public static int GetColorInt(MyColor color) { return (int)color; }

    public static Color Red { get { return colors[(int)MyColor.Red]; } }
    public static Color Green { get { return colors[(int)MyColor.Green]; } }
    public static Color Blue { get { return colors[(int)MyColor.Blue]; } }
}

//도망자 상태
public enum State
{
    Alive,
    Dead
}


//역할
public enum Role
{
    Runner,
    Tagger
}