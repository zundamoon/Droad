using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSquareData
{
    public Color squareColor { get; protected set; }
    public int eventID { get; protected set; }

    public BaseSquareData()
    {
        squareColor = Color.white;
        eventID = -1;
    }

    public void ChangeColor(Color color) { squareColor = color; }
}
