using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event027_LoseHalfCoinToAdvance : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        CardData card = context.card;
        if (character == null || card == null) return;

        int halfCoin = character.coins / 2;
        character.RemoveCoin(halfCoin);

        CardData newCard = new CardData();
        newCard.SetAdvance(halfCoin);
        context.card = newCard;

        await UniTask.CompletedTask;
    }
}
