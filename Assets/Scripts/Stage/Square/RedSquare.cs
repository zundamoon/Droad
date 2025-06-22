using UnityEngine;

public class RedSquare : BaseSquareData
{
    public override void Initialize()
    {
        squareColor = Color.red;
        eventID = 8;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}

