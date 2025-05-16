using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event000_DiscardHand : BaseEvent
{
    public override async UniTask PlayEvent(Character sourceCharacter, int param, Square square = null)
    {
        // �I���̌Ăяo��
        int handIndex = 0;
        // �I�����ꂽ�J�[�h���̂Ă�
        sourceCharacter.possessCard.DiscardHand(handIndex);
        await UniTask.DelayFrame(1);
    }
}
