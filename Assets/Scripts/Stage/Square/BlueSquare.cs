using UnityEngine;

public class BlueSquare : BaseSquareData
{
    public BlueSquare()
    {
        squareColor = Color.blue;
        eventID = 6;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}
