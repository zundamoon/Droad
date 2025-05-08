using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Entity_CardData;

public class CardManager
{
    public static List<Card> cardList { get; private set; } = null;

    private const int _CARD_MAX = 24;

    public static void Init()
    {
        // マスターデータからカードを生成
        cardList = new List<Card>();
        for (int i = 0; i < _CARD_MAX; i++)
        {
            Param cardMaster = CardMasterUtility.GetCardMaster(i);
            if (cardMaster == null) return;

            Card addCard = new Card();
            addCard.Init(i, cardMaster.advance, cardMaster.coin, (GameEnum.Rarity)cardMaster.rarity, cardMaster.eventID);
            cardList.Add(addCard);
        }
    }
}
