using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event100_BuyStar : BaseEvent
{
    public override async UniTask PlayEvent(Character sourceCharacter, int param, Square square = null)
    {
        // �w�����邩�I��
        if (false) return;
        // �J�[�h�ǉ�
        sourceCharacter.RemoveCoin(param);
        int cardID = -1;
        await sourceCharacter.possessCard.AddCard(cardID);
        // �X�^�[�}�X�̈ʒu�ύX

    }
}
