using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event001_DiscardHand : BaseEvent
{
    public override void PlayEvent(Character sourceCharacter, int param, int addParam = -1)
    {
        // �I���̌Ăяo��
        int handIndex = 0;
        // �I�����ꂽ�J�[�h���̂Ă�
        sourceCharacter.possessCard.DiscardHand(handIndex);
    }
}
