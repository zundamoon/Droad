using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidSquare : Square
{
    public override int GetEventID()
    {
        return -1;
    }
}

