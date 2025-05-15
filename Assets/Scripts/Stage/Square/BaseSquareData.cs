using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSquareData
{
    public Color squareColor;
    public int eventID;

    public BaseSquareData()
    {
        squareColor = Color.white;
        eventID = -1;
    }
}
