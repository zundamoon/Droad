using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event006_AddCoin : BaseEvent
{
    private const int _ADD_COIN_TEXT_ID = 105;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        await UIManager.instance.RunMessage(string.Format(_ADD_COIN_TEXT_ID.ToText(), param));
        Character character = context.character;
        if (character == null) return;

        character.AddCoin(param);
    }
}
