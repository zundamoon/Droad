using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;

public class ShopSquare : Square
{
    public override EventID GetEventID()
    {
        return EventID.SHOP;
    }
}
