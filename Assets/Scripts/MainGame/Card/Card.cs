using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public int ID { get; private set; } = -1;
    public int advance { get; private set; } = 0;
    public int addCoin { get; private set; } = 0;
    public GameEnum.Rarity rarity { get; private set; } = 0;
    public BaseEvent playEvent { get; private set; } = null;
    
    public void Init(int setID, int setAdvance, int setAddCoin, GameEnum.Rarity setRarity, BaseEvent setEvent)
    {
        ID = setID;
        advance = setAdvance;
        addCoin = setAddCoin;
        rarity = setRarity;
        playEvent = setEvent;
    }
}
