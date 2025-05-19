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

    public enum EventID
    {
        INVALID = -1,
        DISCARD_HAND = 0,
        STEAL_COIN = 1,
        RESHUFFLE = 2,
        DISCARD_DECK = 3,
        CANCEL_NEXT_SQUARE_EVENT = 4,
        DISCARD_DECK_1EXE = 5,
        ADD_COIN = 6,
        BUY_STAR = 100,
        SHOP = 101,
    }
}
