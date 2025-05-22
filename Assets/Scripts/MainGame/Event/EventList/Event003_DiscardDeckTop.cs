using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event003_DiscardDeckTop : BaseEvent
{
    private const int _TEXT_ID = 116;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        await character.possessCard.DiscardDeckTop(param);
        await UIManager.instance.RunMessage(string.Format(_TEXT_ID.ToText(), param));
    }
}
