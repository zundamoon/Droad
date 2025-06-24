using UnityEngine;

public class InvalidSquare : BaseSquareData
{
    public override void Initialize()
    {
        squareType = GameEnum.SquareType.INVALID;
        eventID = -1;
        isStopSquare = false;
    }
}

