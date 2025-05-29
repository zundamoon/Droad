using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;

public class Event028_GiftCard : BaseEvent
{
    /// <summary>
    /// ���A���e�B�ƒ��I����
    /// </summary>
    private static Dictionary<Rarity, int> rarityWeights = new Dictionary<Rarity, int>
    {
        { Rarity.SILVER, 50 },
        { Rarity.GOLD, 30 },
        { Rarity.LEGENDARY, 20 }
    };

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        int cardID = CardManager.instance.GetRandRarityCard(GetRarity());
        await character.possessCard.AddCardDiscard(cardID);
    }

    /// <summary>
    /// �����_���ȃ��A���e�B���擾
    /// </summary>
    /// <returns></returns>
    private Rarity GetRarity()
    {
        // �d�݂��擾
        int totalRatio = 0;
        foreach (var weight in rarityWeights.Values)
        {
            totalRatio += weight;
        }
        int randomValue = Random.Range(0, totalRatio);

        // �d�݂��烌�A���e�B��I�o
        int currentRatio = 0;
        foreach (var pair in rarityWeights)
        {
            currentRatio += pair.Value;
            if (randomValue < currentRatio)
            {
                return pair.Key;
            }
        }
        return Rarity.INVALID;
    }
}
