using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event003_DiscardDeck : BaseEvent
{
    public override async UniTask PlayEvent(Character sourceCharacter, int param, Square square = null)
    {
        sourceCharacter.possessCard.DiscardDeck(param);
    }
}
