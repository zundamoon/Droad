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
        LUCKY,
        UNLUCKY,
        GIFT,
        HAPPENING,
        SHOP,
        BRANCH,
        STAR,
        MAX
    }

    public enum BGM
    {
        TITLE = 0,
        MAIN,
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
        BUY_SHOP,
        GET_STAR,
        POPUP,
        MAX
    }
}
