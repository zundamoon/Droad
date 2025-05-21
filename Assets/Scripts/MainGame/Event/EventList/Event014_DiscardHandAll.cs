using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event014_DiscardHandAll : BaseEvent
{
    private const int _TEXT_ID = 117;
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        character.possessCard.DiscardHandAll();
        await UIManager.instance.RunMessage(_TEXT_ID.ToText());
    }
}
