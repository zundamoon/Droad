using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event005_DiscardDeck1Exe : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        PossessCard sourcePossessCard = character.possessCard;
        int discardID = sourcePossessCard.deckCardIDList[0];
        sourcePossessCard.DiscardDeck(1);
        // Ì‚Ä‚½ƒJ[ƒh‚ÌID‚©‚çŒø‰Ê”­“®
        int eventID = CardMasterUtility.GetCardMaster(discardID).eventID;
        await EventManager.ExecuteEvent(eventID, context);
    }
}
