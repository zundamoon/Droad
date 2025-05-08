using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager
{
    public List<Card> cardList { get; private set; } = null;

    public void Init()
    {
        // マスターデータからカードを生成
        cardList = new List<Card>();
        
    }
}
