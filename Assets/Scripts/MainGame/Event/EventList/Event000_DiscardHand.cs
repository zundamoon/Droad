using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event000_DiscardHand : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        // �I���̌Ăяo��
        int handIndex = 0;
        Character character = context.character;
        if (character == null) return;

        // �I�����ꂽ�J�[�h���̂Ă�
        character.possessCard.DiscardHand(handIndex);
        await UniTask.CompletedTask;
    }
}
