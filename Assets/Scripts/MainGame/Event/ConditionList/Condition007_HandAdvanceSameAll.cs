using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition007_HandAdvanceSameAll : BaseCondition
{
    public override async UniTask<bool> IsCompleteCondition(EventContext context, int param)
    {
        if (context == null) return false;

        Character character = context.character;
        if (character == null) return false;

        List<int> handIDList = character.possessCard.handCardIDList;
        int handCount = handIDList.Count;
        // ��D��1���ȉ��Ȃ�true
        if (handCount <= 1) return true;
        int beforeAdvance = CardManager.GetCard(handIDList[0]).advance;
        for (int i = 1, max = handIDList.Count; i < max; i++)
        {
            int advance = CardManager.GetCard(handIDList[i]).advance;

            if (advance != beforeAdvance) return false;
        }

        return true;
    }
}
