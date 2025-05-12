using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event006_AddCoin : BaseEvent
{
    public override void PlayEvent(Character sourceCharacter, int param, int addParam = -1)
    {
        sourceCharacter.AddCoin(param);
    }
}
