using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition021_HandAdvanceSame : BaseCondition
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
            int advance = CardManager.instance.GetCard(handList[i]).advance;
            if (advance == param) return true;
        }
        return false;
    }
}