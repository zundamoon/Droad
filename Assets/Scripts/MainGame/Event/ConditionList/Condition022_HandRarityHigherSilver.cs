using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;

public class Condition022_HandRarityHigherSilver : BaseCondition
{
    public override async UniTask<bool> IsCompleteCondition(EventContext context, int param)
    {
        if (context == null) return false;

        Character character = context.character;
        if (character == null) return false;

        List<int> handList = character.possessCard.handCardIDList;
        int handCount = handList.Count;
        for (int i = 0; i < handCount; i++)
        {
            Rarity rarity = CardManager.instance.GetCard(handList[i]).rarity;
            if (rarity < Rarity.SILVER) return false;
        }
        return true;
    }
}