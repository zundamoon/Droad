using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event005_CancelNextSquareEvent : BaseEvent
{
    public override void PlayEvent(Character sourceCharacter, int param)
    {
        sourceCharacter.SetCancelEvent();
    }
}
