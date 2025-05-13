using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event005_DiscardDeck1Exe : BaseEvent
{
    public override async UniTask PlayEvent(Character sourceCharacter, int param, int addParam = -1)
    {
        PossessCard sourcePossessCard = sourceCharacter.possessCard;
        int discardID = sourcePossessCard.deckCardIDList[0];
        sourcePossessCard.DiscardDeck(1);
        // Ì‚Ä‚½ƒJ[ƒh‚ÌID‚©‚çŒø‰Ê”­“®
        int eventID = CardMasterUtility.GetCardMaster(discardID).eventID;
        EventManager.ExecuteEvent(sourceCharacter, eventID);
    }
}
