using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition017_HandAdvanceSumHigher : BaseCondition
{
    public override async UniTask<bool> IsCompleteCondition(EventContext context, int param)
    {
        if (context == null) return false;

        Character character = context.character;
        if (character == null) return false;

        List<int> handList = character.possessCard.handCardIDList;
        int handCount = handList.Count;
        int advanceSum = 0;
        for (int i = 0; i < handCount; i++)
        {
            advanceSum += CardManager.instance.GetCard(handList[i]).advance;
        }

        return advanceSum >= param;
    }
}