using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event100_BuyStar : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // �w�����邩�I��
        if (false) return;
        // �J�[�h�ǉ�
        character.RemoveCoin(param);
        int cardID = -1;
        await character.possessCard.AddCard(cardID);
        // �X�^�[�}�X�̈ʒu�ύX

    }
}
