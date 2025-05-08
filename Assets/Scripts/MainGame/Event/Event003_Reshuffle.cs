using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event003_Reshuffle : BaseEvent
{
    public override void PlayEvent(Character sourceCharacter, int param)
    {
        sourceCharacter.possessCard.ReshuffleDeck();
    }
}
