using UnityEngine;

public class HappeningSquare : BaseSquareData
{
    public override void Initialize()
    {
        squareType = GameEnum.SquareType.HAPPENING;
        eventID = -1;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}

