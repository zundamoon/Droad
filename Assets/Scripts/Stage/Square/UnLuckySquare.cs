using UnityEngine;

public class UnLuckySquare : BaseSquareData
{
    public override void Initialize()
    {
        squareColor = Color.black;
        eventID = 11;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}
