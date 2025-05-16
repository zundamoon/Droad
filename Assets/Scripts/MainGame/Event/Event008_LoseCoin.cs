using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event008_LoseCoin : BaseEvent
{
    private const int _LOSE_COIN_TEXT_ID = 104;

    public override async UniTask PlayEvent(Character sourceCharacter, int param, Square square = null)
    {
        await UIManager.instance.RunMessage(string.Format(_LOSE_COIN_TEXT_ID.ToText(), param));
        sourceCharacter.RemoveCoin(param);
    }
}
