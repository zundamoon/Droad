using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event024_ExecuteHandCard : BaseEvent
{
    private const int _CHOICE_TEXT_ID = 126;
    private const int _NO_HAND_TEXT_ID = 124;

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
            // �C�x���g���s
            int eventID = CardManager.instance.GetCard(cardID).eventID;
            await EventManager.ExecuteEvent(eventID, context);
        });

        await UIManager.instance.RunMessage(_CHOICE_TEXT_ID.ToText());
        await UIManager.instance.OpenChoiceArea(character.possessCard.handCardIDList);
    }
}
