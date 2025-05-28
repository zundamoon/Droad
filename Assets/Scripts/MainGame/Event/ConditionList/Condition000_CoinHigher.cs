using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Condition000_CoinHigher : BaseCondition
{
    public override async UniTask<bool> IsCompleteCondition(EventContext context, int param)
    {
        if (context == null) return false;

        Character character = context.character;
        if (character == null) return false;

        return character.coins >= param;
    }
}
