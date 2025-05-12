using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event100_BuyStar : BaseEvent
{
    public override void PlayEvent(Character sourceCharacter, int param, int addParam = -1)
    {
        // 購入できるか判定
        if (sourceCharacter.coins < param) return;
        // 購入するか選択
        if (false) return;

        sourceCharacter.RemoveCoin(param);
        sourceCharacter.possessCard.AddCard(addParam);

        // スターの位置変更

    }
}
