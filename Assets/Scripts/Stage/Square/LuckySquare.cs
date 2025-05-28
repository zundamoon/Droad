using UnityEngine;

public class LuckySquare : BaseSquareData
{
    private Color yellow = new(1.0f, 1.0f, 0.5f, 1.0f);
    public LuckySquare()
    {
        squareColor = yellow;
        eventID = 10;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}
