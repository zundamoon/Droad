using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event000_DiscardHand : BaseEvent
{
    public override async UniTask PlayEvent(Character sourceCharacter, int param, Square square = null)
    {
        // 選択の呼び出し
        int handIndex = 0;
        // 選択されたカードを捨てる
        sourceCharacter.possessCard.DiscardHand(handIndex);
        await UniTask.DelayFrame(1);
    }
}
