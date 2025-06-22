using UnityEngine;

public class HappeningSquare : BaseSquareData
{
    public override void Initialize()
    {
        squareColor = Color.white;
        eventID = -1;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}

