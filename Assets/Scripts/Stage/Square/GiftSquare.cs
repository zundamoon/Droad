using UnityEngine;

public class GiftSquare : BaseSquareData
{
    private Color orange = new Color(1.0f, 0.5f, 0.25f, 1.0f);
    public GiftSquare()
    {
        squareColor = orange;
        eventID = 29;
        isStopSquare = false;
        canRepeatSquare = true;
    }
}
