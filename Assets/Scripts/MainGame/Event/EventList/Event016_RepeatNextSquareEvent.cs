using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event016_RepeatNextSquareEvent : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // �L�����N�^�[�̃C�x���g�J��Ԃ���ݒ�
        character.SetRepeatEventCount(param);

        await UniTask.CompletedTask;
    }
}
