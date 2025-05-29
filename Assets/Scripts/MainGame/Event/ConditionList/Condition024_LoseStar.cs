using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition024_LoseStar : BaseCondition
{
    public override async UniTask<bool> IsCompleteCondition(EventContext context, int param)
    {
        if (context == null) return false;

        Character character = context.character;
        if (character == null) return false;

        if (character.stars < param) return false;

        for (int i = 0; i < param; i++)
        {
            character.possessCard.RemoveRandomStarCard();
        }
        return true;
    }
}