using UnityEngine;

public class BlueSquare : BaseSquareData
{
    public override void Initialize()
    {
        squareColor = Color.blue;
        eventID = 6;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}
