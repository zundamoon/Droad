using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event100_BuyStar : BaseEvent
{
    public override void PlayEvent(Character sourceCharacter, int param, int addParam = -1)
    {
        // �w���ł��邩����
        if (sourceCharacter.coins < param) return;
        // �w�����邩�I��
        if (false) return;

        sourceCharacter.RemoveCoin(param);
        sourceCharacter.possessCard.AddCard(addParam);

        // �X�^�[�̈ʒu�ύX

    }
}
