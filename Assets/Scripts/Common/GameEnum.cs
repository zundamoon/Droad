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
        STAR,
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

    public enum BGM
    {
        
        MAX
    }

    public enum SE
    {
        DRAW_CARD = 0,
        USE_CARD,
        UI_ADVANCE,
        PLAYER_ADVANCE,
        PLAYER_LANDING,
        START,
        SELECT_1,
        SELECT_2,
        CANCEL,
        GET_COIN,
        MAX
    }
}
