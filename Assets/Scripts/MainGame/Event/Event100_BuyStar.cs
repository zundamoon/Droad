using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event100_BuyStar : BaseEvent
{
    public override async UniTask PlayEvent(Character sourceCharacter, int param, Square square = null)
    {
        // 購入するか選択
        if (false) return;
        // カード追加
        sourceCharacter.RemoveCoin(param);
        int cardID = -1;
        await sourceCharacter.possessCard.AddCard(cardID);
        // スターマスの位置変更

    }
}
