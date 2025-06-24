using UnityEngine;

public class LuckySquare : BaseSquareData
{
    public override void Initialize()
    {
        squareType = GameEnum.SquareType.LUCKY;
        eventID = 10;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}
