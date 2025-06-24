using UnityEngine;

public class RedSquare : BaseSquareData
{
    public override void Initialize()
    {
        squareType = GameEnum.SquareType.RED;
        eventID = 8;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}

