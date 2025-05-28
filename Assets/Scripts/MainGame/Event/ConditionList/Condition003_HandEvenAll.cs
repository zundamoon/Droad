using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition003_HandEvenAll : BaseCondition
{
    public override async UniTask<bool> IsCompleteCondition(EventContext context, int param)
    {
        if (context == null) return false;

        Character character = context.character;
        if (character == null) return false;

        List<int> handIDList = character.possessCard.handCardIDList;
        for (int i = 0, max = handIDList.Count; i < max; i++)
        {
            int advance = CardManager.GetCard(handIDList[i]).advance;
            // �����łȂ��Ȃ�false
            if (advance % 2 != 0) return false;
        }

        return true;
    }
}
