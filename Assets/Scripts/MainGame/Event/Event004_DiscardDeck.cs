using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event004_DiscardDeck : BaseEvent
{
    public override void PlayEvent(Character sourceCharacter, int param)
    {
        sourceCharacter.possessCard.DiscardDeck(param);
    }
}
