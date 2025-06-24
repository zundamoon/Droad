using UnityEngine;

public class GiftSquare : BaseSquareData
{
    public override void Initialize()
    {
        squareType = GameEnum.SquareType.GIFT;
        eventID = 29;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}
