using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;

public class Event028_GiftCard : BaseEvent
{
    private readonly int[] _RARITY_RATIO = { 25, 25, 25, 25 };

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        int cardID = CardManager.GetRandRarityCard(GetRarity());
        await character.possessCard.AddCardDiscard(cardID);
    }

    private Rarity GetRarity()
    {
        // 重みを取得
        int rarityMax = _RARITY_RATIO.Length;
        int totalRatio = 0;
        for (int i = 0; i < rarityMax; i++)
        {
            totalRatio += _RARITY_RATIO[i];
        }
        int randomvalue = Random.Range(0, totalRatio);

        // 重みからレアリティを選出
        int currentRatio = 0;
        Rarity rarity = Rarity.INVALID;
        for (int i = 0; i < rarityMax; i++)
        {
            currentRatio += _RARITY_RATIO[i];
            if (randomvalue < currentRatio)
            {
                return (Rarity)i;
            }
        }
        return rarity;
    }
}
