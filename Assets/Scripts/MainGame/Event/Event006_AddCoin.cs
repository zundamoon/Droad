using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event006_AddCoin : BaseEvent
{
    public override async UniTask PlayEvent(Character sourceCharacter, int param, int addParam = -1)
    {
        sourceCharacter.AddCoin(param);
    }
}
