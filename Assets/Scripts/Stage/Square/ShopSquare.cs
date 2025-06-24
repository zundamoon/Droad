using UnityEngine;

public class ShopSquare : BaseSquareData
{
    public override void Initialize()
    {
        squareType = GameEnum.SquareType.SHOP;
        eventID = 13;
        isStopSquare = false;
        canRepeatSquare = false;
    }
}
