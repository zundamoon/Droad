using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event100_BuyStar : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // 購入するか選択
        if (false) return;
        // カード追加
        character.RemoveCoin(param);
        int cardID = -1;
        await character.possessCard.AddCard(cardID);
        // スターマスの位置変更

    }
}
