using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event005_DiscardDeck1Exe : BaseEvent
{
    public override async UniTask PlayEvent(Character sourceCharacter, int param, Square square = null)
    {
        PossessCard sourcePossessCard = sourceCharacter.possessCard;
        int discardID = sourcePossessCard.deckCardIDList[0];
        sourcePossessCard.DiscardDeck(1);
        // �̂Ă��J�[�h��ID������ʔ���
        int eventID = CardMasterUtility.GetCardMaster(discardID).eventID;
        await EventManager.ExecuteEvent(sourceCharacter, eventID);
    }
}
