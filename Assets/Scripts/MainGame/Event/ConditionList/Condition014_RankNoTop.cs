using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition014_RankNoTop : BaseCondition
{
    public override async UniTask<bool> IsCompleteCondition(EventContext context, int param)
    {
        if (context == null) return false;

        Character character = context.character;
        if (character == null) return false;

        int rank = character.rank;

        return rank != 1;
    }
}
