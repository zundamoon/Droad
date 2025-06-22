using UnityEngine;

public class InvalidSquare : BaseSquareData
{
    public override void Initialize()
    {
        squareColor = Color.grey;
        eventID = -1;
        isStopSquare = false;
    }
}

