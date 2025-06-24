using UnityEngine;

public class BlueSquare : BaseSquareData
{
    public override void Initialize()
    {
        squareType = GameEnum.SquareType.BLUE;
        eventID = 6;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}
