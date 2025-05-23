using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonModule;

public abstract class BaseSquareData
{
    // ステージ上のマスの位置
    public StagePosition squarePosition;
    // 次に移動できるマスの候補
    public List<StagePosition> nextPositionList;
    // マスの色
    public Color squareColor { get; protected set; }
    // イベントID
    public int eventID { get; protected set; }

    public bool isStarSquare;
    public bool isStopSquare { get; protected set; }
    public bool canRepeatSquare { get; protected set; }


    public BaseSquareData()
    {
        squareColor = Color.white;
        eventID = -1;
    }

    public void ChangeColor(Color color) { squareColor = color; }
}
