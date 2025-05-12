using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event006_DiscardDeck1Exe : BaseEvent
{
    public override void PlayEvent(Character sourceCharacter, int param, int addParam = -1)
    {
        PossessCard sourcePossessCard = sourceCharacter.possessCard;
        int discardID = sourcePossessCard.deckCardIDList[0];
        sourcePossessCard.DiscardDeck(1);
        // Ì‚Ä‚½ƒJ[ƒh‚ÌID‚©‚çŒø‰Ê”­“®
        int eventID = CardMasterUtility.GetCardMaster(discardID).eventID;
        EventManager.ExecuteEvent(sourceCharacter, eventID);
    }
}
