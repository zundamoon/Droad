using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.TextCore.Text;

public class Event012_BuyStar : BaseEvent
{
    private int[] _starCardIDList = { 24, 25, 26, 27, 28, 29 };

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        CardData card = context.card;
        if (character == null || card == null) return;

        // UI•\¦

        // SetCallback(
        //  character.RemoveCoin(card.price);
        //  await character.possessCard.AddCard(card.ID);
        //  acceptEnd = true;
        // )
        bool acceptEnd = false;
        // ƒLƒƒƒ“ƒZƒ‹‚Ìİ’è
        // SetCalback(
        //  acceptEnd = true;
        // )

        // “ü—Í‘Ò‚¿
        while (!acceptEnd)
        {
            await UniTask.DelayFrame(1);
        }
    }
}
