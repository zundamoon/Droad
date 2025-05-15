using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;

public class RedSquare : Square
{
    public override EventID GetEventID()
    {
        return EventID.STEAL_COIN;
    }
}

