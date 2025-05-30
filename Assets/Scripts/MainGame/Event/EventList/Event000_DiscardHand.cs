using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event000_DiscardHand : BaseEvent
{
    private const int _CHOICE_TEXT_ID = 114;
    private const int _NO_HAND_TEXT_ID = 124;
    private const int _DISCARD_HAND_TEXT_ID = 132;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // ��D���Ȃ��Ȃ珈�����Ȃ�
        if (character.possessCard.handCardIDList.Count <= 0)
        {
            await UIManager.instance.RunMessage(_NO_HAND_TEXT_ID.ToText());
            return;
        }

        // �I���̌Ăяo��
        await UIManager.instance.SetChoiceCallback(async (cardID) =>
        {
            await character.possessCard.DiscardHandID(cardID);
            await UIManager.instance.RunMessage(_DISCARD_HAND_TEXT_ID.ToText());
        });

        await UIManager.instance.RunMessage(_CHOICE_TEXT_ID.ToText());
        await UIManager.instance.OpenChoiceArea(character.possessCard.handCardIDList);
        await UniTask.CompletedTask;
    }
}
