using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event032_LoseHalfCoin : BaseEvent
{
    private const int _LOSE_HALF_COIN_TEXT_ID = 136;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        int halfCoin = character.coins / 2;
        character.RemoveCoin(halfCoin);
        await UIManager.instance.RunMessage(_LOSE_HALF_COIN_TEXT_ID.ToText());
    }
}
