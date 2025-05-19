using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event008_LoseCoin : BaseEvent
{
    private const int _LOSE_COIN_TEXT_ID = 104;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        await UIManager.instance.RunMessage(string.Format(_LOSE_COIN_TEXT_ID.ToText(), param));
        character.RemoveCoin(param);
    }
}
