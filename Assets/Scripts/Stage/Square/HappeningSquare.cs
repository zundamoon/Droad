using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;

public class HappeningSquare : Square
{
    public override EventID GetEventID()
    {
        return eventID;
    }
}

