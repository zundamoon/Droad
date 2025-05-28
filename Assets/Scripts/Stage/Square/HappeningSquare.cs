using UnityEngine;

public class HappeningSquare : BaseSquareData
{
    public HappeningSquare()
    {
        squareColor = Color.white;
        eventID = -1;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}

