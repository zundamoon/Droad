using UnityEngine;

public class HappeningSquare : BaseSquareData
{
    public HappeningSquare()
    {
        squareColor = Color.green;
        eventID = 2;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}

