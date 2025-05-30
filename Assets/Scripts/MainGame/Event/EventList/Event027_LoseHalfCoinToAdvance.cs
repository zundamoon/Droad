using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event027_LoseHalfCoinToAdvance : BaseEvent
{
    private const int _LOSE_HALF_COIN_TEXT_ID = 136;
    private const int _ADVANCE_TEXT_ID = 137;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        CardData card = context.card;
        if (character == null || card == null) return;

        int halfCoin = character.coins / 2;
        character.RemoveCoin(halfCoin);
        await UIManager.instance.RunMessage(_LOSE_HALF_COIN_TEXT_ID.ToText());

        CardData newCard = new CardData();
        newCard.SetAdvance(halfCoin);
        context.card = newCard;
        await UIManager.instance.RunMessage(string.Format(_ADVANCE_TEXT_ID.ToText(), halfCoin));

        await UniTask.CompletedTask;
    }
}
