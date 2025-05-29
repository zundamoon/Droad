using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

using static Entity_CardData;
using static CommonModule;
using static GameEnum;

public class CardManager : SystemObject
{
    [SerializeField]
    private SpriteClip _spriteClip = null;
    public List<CardData> cardList { get; private set; } = null;
    public List<List<int>> rarityCardIDList { get; private set; } = null;
    public static CardManager instance { get; private set; } = null;

    private const int _CARD_MAX = 30;

    public override async UniTask Initialize()
    {
        instance = this;
        // マスターデータからカードを生成
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
            addCard.Init(i, cardMaster.nameID, cardMaster.imageID, cardMaster.advance, cardMaster.coin, cardMaster.star, 
                (Rarity)cardMaster.rarity, cardMaster.eventID, cardMaster.price);
            cardList.Add(addCard);
            rarityCardIDList[cardMaster.rarity].Add(i);
        }
    }

    public CardData GetCard(int ID)
    {
        if (!IsEnableIndex(cardList, ID)) return null;

        return cardList[ID];
    }

    /// <summary>
    /// レアリティ指定でランダムなカードIDを取得する
    /// </summary>
    /// <param name="rarity"></param>
    /// <returns></returns>
    public int GetRandRarityCard(Rarity rarity)
    {
        List<int> cardList = rarityCardIDList[(int)rarity];
        int randIndex = Random.Range(0, cardList.Count);
        return cardList[randIndex];
    }

    /// <summary>
    /// カードスプライトを取得
    /// </summary>
    /// <param name="spriteID"></param>
    /// <returns></returns>
    public Sprite GetCardImage(int spriteID)
    {
        Sprite[] spriteClip = _spriteClip.spriteClip;
        if (!IsEnableIndex(spriteClip, spriteID)) return null;
        return spriteClip[spriteID];
    }
}
