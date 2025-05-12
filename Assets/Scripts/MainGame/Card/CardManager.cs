using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Entity_CardData;

public class CardManager
{
    public static List<CardData> cardList { get; private set; } = null;

    private const int _CARD_MAX = 24;

    public static void Init()
    {
        // マスターデータからカードを生成
        cardList = new List<CardData>();
        for (int i = 0; i < _CARD_MAX; i++)
        {
            Param cardMaster = CardMasterUtility.GetCardMaster(i);
            if (cardMaster == null) return;

            CardData addCard = new CardData();
            addCard.Init(i, cardMaster.advance, cardMaster.coin, cardMaster.star, (GameEnum.Rarity)cardMaster.rarity, cardMaster.eventID);
            cardList.Add(addCard);
        }
    }

    public static CardData GetCard(int ID)
    {
        return cardList[ID];
    }
}
