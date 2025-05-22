using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event023_ExecuteDiscardCard : BaseEvent
{
    private const int _CHOICE_TEXT_ID = 123;
    private const int _NO_DISCARD_TEXT_ID = 125;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // �̂ĎD���Ȃ��Ȃ珈�����Ȃ�
        if (character.possessCard.discardCardIDList.Count <= 0)
        {
            await UIManager.instance.RunMessage(_NO_DISCARD_TEXT_ID.ToText());
            return;
        }

        // �I���̌Ăяo��
        await UIManager.instance.SetChoiceCallback(async (cardID) =>
        {
            // �f�b�L�ɖ߂�
            character.possessCard.ReturnDiscardToDeck(cardID);
            // �C�x���g���s
            int eventID = CardManager.GetCard(cardID).eventID;
            await EventManager.ExecuteEvent(eventID, context);
        });

        await UIManager.instance.RunMessage(_CHOICE_TEXT_ID.ToText());
        await UIManager.instance.OpenChoiceArea(character.possessCard.discardCardIDList);
    }
}
