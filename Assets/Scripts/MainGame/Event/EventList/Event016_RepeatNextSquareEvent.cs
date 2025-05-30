using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event016_RepeatNextSquareEvent : BaseEvent
{
    private const int _TEXT_ID = 135;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        await UIManager.instance.RunMessage(string.Format(_TEXT_ID.ToText(), param));
        // キャラクターのイベント繰り返しを設定
        character.SetRepeatEventCount(param);
    }
}
