using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event022_DiscardHandAdvance : BaseEvent
{
    private const int _CHOICE_TEXT_ID = 114;
    private const int _NO_HAND_TEXT_ID = 124;
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        CardData card = context.card;
        if (character == null || card == null) return;

        // èD‚ª‚È‚¢‚È‚çˆ—‚µ‚È‚¢
        if (character.possessCard.handCardIDList.Count <= 0)
        {
            await UIManager.instance.RunMessage(_NO_HAND_TEXT_ID.ToText());
            return;
        }

        // ‘I‘ğ‚ÌŒÄ‚Ño‚µ
        await UIManager.instance.SetChoiceCallback((cardID) =>
        {
            character.possessCard.DiscardHandID(cardID);
            CardData newCard = new CardData();
            newCard.SetAdvance(card.advance + CardManager.instance.GetCard(cardID).advance);
            context.card = newCard;
        });

        await UIManager.instance.RunMessage(_CHOICE_TEXT_ID.ToText());
        await UIManager.instance.OpenChoiceArea(character.possessCard.handCardIDList);
    }
}
