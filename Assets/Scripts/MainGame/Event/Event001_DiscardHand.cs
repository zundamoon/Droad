using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event001_DiscardHand : BaseEvent
{
    public override void PlayEvent(Character sourceCharacter, int param)
    {
        // 選択の呼び出し
        int handIndex = 0;
        // 選択されたカードを捨てる
        sourceCharacter.possessCard.DiscardHand(handIndex);
    }
}
