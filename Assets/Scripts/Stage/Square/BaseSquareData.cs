using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonModule;
using static GameEnum;

public abstract class BaseSquareData
{
    public SquareType squareType = SquareType.INVALID;
    // ステージ上のマスの位置
    public StagePosition squarePosition;
    // 次に移動できるマスの候補
    public List<StagePosition> nextPositionList;
    // イベントID
    public int eventID { get; protected set; }

    public bool isStarSquare;
    public bool isStopSquare { get; protected set; }
    public bool canRepeatSquare { get; protected set; }

    public virtual void Initialize()
    {
        eventID = -1;
    }
}
