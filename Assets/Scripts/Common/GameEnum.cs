using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnum
{
    public enum Rarity
    {
        INVALID = -1,
        BRONZE,
        SILVER,
        GOLD,
        LEGENDARY,
        MAX
    }

    public enum SquareType
    {
        INVALID = -1,
        BLUE,
        RED,
        HAPPENING,
        SHOP,
        MAX
    }
}
