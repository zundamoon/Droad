using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition005_HandAdvanceHigherAll : BaseCondition
{
    public override async UniTask<bool> IsCompleteCondition(EventContext context, int param)
    {
        if (context == null) return false;

        Character character = context.character;
        if (character == null) return false;

        List<int> handIDList = character.possessCard.handCardIDList;
        for (int i = 0, max = handIDList.Count; i < max; i++)
        {
            int advance = CardManager.instance.GetCard(handIDList[i]).advance;
            if (advance < param) return false;
        }

        return true;
    }
}
