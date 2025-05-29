using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;

public class Condition008_HandRaritySameAll : BaseCondition
{
    public override async UniTask<bool> IsCompleteCondition(EventContext context, int param)
    {
        if (context == null) return false;

        Character character = context.character;
        if (character == null) return false;

        List<int> handIDList = character.possessCard.handCardIDList;
        int handCount = handIDList.Count;
        // ŽèŽD‚ª1–‡ˆÈ‰º‚È‚çtrue
        if (handCount <= 1) return true;
        Rarity beforeRarity = CardManager.instance.GetCard(handIDList[0]).rarity;
        for (int i = 1, max = handIDList.Count; i < max; i++)
        {
            Rarity rarity = CardManager.instance.GetCard(handIDList[i]).rarity;

            if (rarity != beforeRarity) return false;
        }

        return true;
    }
}
