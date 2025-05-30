using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event004_CancelNextSquareEvent : BaseEvent
{
    private const int _TEXT_ID = 131;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        character.SetCancelEvent();
        await UIManager.instance.RunMessage(_TEXT_ID.ToText());
    }
}
