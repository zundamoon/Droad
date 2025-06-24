using UnityEngine;

public class UnLuckySquare : BaseSquareData
{
    public override void Initialize()
    {
        squareType = GameEnum.SquareType.UNLUCKY;
        eventID = 11;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}
