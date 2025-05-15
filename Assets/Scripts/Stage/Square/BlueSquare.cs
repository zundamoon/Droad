using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;

public class BlueSquare : Square
{
    public override EventID GetEventID()
    {
        return EventID.ADD_COIN;
    }
}
