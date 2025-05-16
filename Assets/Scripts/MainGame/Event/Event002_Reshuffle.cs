using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event002_Reshuffle : BaseEvent
{

    public override async UniTask PlayEvent(Character sourceCharacter, int param, Square square = null)
    {
        await sourceCharacter.possessCard.ReshuffleDeck();
    }
}
