using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;

public class InvalidSquare : Square
{
    public override EventID GetEventID()
    {
        return EventID.INVALID;
    }
}

