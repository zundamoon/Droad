using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition002_HandStarHigher : BaseCondition
{
    public override async UniTask<bool> IsCompleteCondition(EventContext context, int param)
    {
        if (context == null) return false;

        Character character = context.character;
        if (character == null) return false;

        List<int> handIDList = character.possessCard.handCardIDList;
        int starCount = 0;
        for (int i = 0, max = handIDList.Count; i < max; i++)
        {
            if (!CardManager.instance.GetCard(handIDList[i]).IsStar()) continue;
            starCount++;
        }

        return starCount >= param;
    }
}

