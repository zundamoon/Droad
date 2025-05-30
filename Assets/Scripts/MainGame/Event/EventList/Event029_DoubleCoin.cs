using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event029_DoubleCoin : BaseEvent
{
    private const int _DOUBLE_COIN_TEXT_ID = 138;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        int coin = character.coins;
        character.AddCoin(coin);
        await UIManager.instance.RunMessage(_DOUBLE_COIN_TEXT_ID.ToText());
    }
}
