using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event100_BuyStar : BaseEvent
{
    public override void PlayEvent(Character sourceCharacter, int param)
    {
        // 購入できるか判定
        if (sourceCharacter.coins < param) return;
        // 購入するか選択
        if (false) return;

        sourceCharacter.RemoveCoin(param);
        sourceCharacter.AddStar(1);

        // スターの位置変更

    }
}
