using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    enum Rarity
    {
        INVALID = -1,
        BRONZE,
        SILVER,
        GOLD,
        LEGENDARY,
        MAX
    }

    private int _advance = 0;
    private int _addCoin = 0;
    private Rarity _rarity;
    private BaseEvent _event;
    
}
