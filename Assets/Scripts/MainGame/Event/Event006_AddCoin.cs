using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event006_AddCoin : BaseEvent
{
    private const int _ADD_COIN_TEXT_ID = 105;

    public override async UniTask PlayEvent(Character sourceCharacter, int param, Square square = null)
    {
        await UIManager.instance.RunMessage(string.Format(_ADD_COIN_TEXT_ID.ToText(), param));
        sourceCharacter.AddCoin(param);
    }
}
