using UnityEngine;

public class ShopSquare : BaseSquareData
{
    private Color yamabuki = new(0.90f, 0.60f, 0.0f, 1.0f);
    public ShopSquare()
    {
        squareColor = yamabuki;
        eventID = 13;
        isStopSquare = false;
        canRepeatSquare = false;
    }
}
