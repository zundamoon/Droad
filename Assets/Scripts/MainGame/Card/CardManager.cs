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
        // �}�X�^�[�f�[�^����J�[�h�𐶐�
        cardList = new List<CardData>(_CARD_MAX);
        List<int> tempList = new List<int>(_CARD_MAX);
        int rarityCount = (int)Rarity.MAX;
        rarityCardIDList = new List<List<int>>(rarityCount);
        for (int i = 0; i < rarityCount; i++)
        {
            rarityCardIDList.Add(new List<int>(_CARD_MAX));
        }

        for (int i = 0; i < _CARD_MAX; i++)
        {
            Param cardMaster = CardMasterUtility.GetCardMaster(i);
            if (cardMaster == null) return;

            CardData addCard = new CardData();
            addCard.Init(i, cardMaster.nameID, cardMaster.advance, cardMaster.coin, cardMaster.star, 
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
    /// ���A���e�B�w��Ń����_���ȃJ�[�hID���擾����
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
