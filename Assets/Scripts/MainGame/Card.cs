using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public int advance { get; private set; } = 0;
    public int addCoin { get; private set; } = 0;
    public GameEnum.Rarity rarity { get; private set; } = 0;
    public BaseEvent playEvent { get; private set; } = null;
    
    public void Init()
    {

    }

    public void Play()
    {
        playEvent.PlayEvent();
    }
}
