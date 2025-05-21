using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event000_DiscardHand : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // ‘I‘ð‚ÌŒÄ‚Ño‚µ
        await UIManager.instance.SetChoiceCallback((cardID) =>
        {
            character.possessCard.DiscardHandID(cardID);
        });

        await UIManager.instance.OpenChoiceArea(character.possessCard.handCardIDList);
        await UniTask.CompletedTask;
    }
}
