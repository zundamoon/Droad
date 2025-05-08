using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event006_DiscardDeck1Exe : BaseEvent
{
    public override void PlayEvent(Character sourceCharacter, int param)
    {
        PossessCard sourcePossessCard = sourceCharacter.possessCard;
        int discardID = sourcePossessCard.deckCardIDList[0];
        sourcePossessCard.DiscardDeck(1);
        // �̂Ă��J�[�h��ID������ʔ���

    }
}
