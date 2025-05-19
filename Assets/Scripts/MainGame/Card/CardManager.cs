using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Entity_CardData;
using static CommonModule;
using static GameEnum;

public class CardManager
{
    public static List<CardData> cardList { get; private set; } = null;
    public static List<List<int>> rarityCardIDList { get; private set; } = null;

    private const int _CARD_MAX = 30;

    public static void Init()
    {
        // マスターデータからカードを生成
        cardList = new List<CardData>(_CARD_MAX);
        rarityCardIDList = new List<List<int>>((int)Rarity.MAX);
        for (int i = 0; i < _CARD_MAX; i++)
        {
            Param cardMaster = CardMasterUtility.GetCardMaster(i);
            if (cardMaster == null) return;

            CardData addCard = new CardData();
            addCard.Init(i, cardMaster.advance, cardMaster.coin, cardMaster.star, 
                (Rarity)cardMaster.rarity, cardMaster.eventID, cardMaster.price);
            cardList.Add(addCard);
            rarityCardIDList[cardMaster.rarity].Add(i);
        }
    }

    public static CardData GetCard(int ID)
    {
        if (!IsEnableIndex(cardList, ID)) return null;

        return cardList[ID];
    }

    /// <summary>
    /// レアリティ指定でランダムなカードIDを取得する
    /// </summary>
    /// <param name="rarity"></param>
    /// <returns></returns>
    public static int GetRandRarityCard(Rarity rarity)
    {
        List<int> cardList = rarityCardIDList[(int)rarity];
        int randIndex = Random.Range(0, cardList.Count);
        return cardList[randIndex];
    }
}
