using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event025_SameAdvanceAddCoin : BaseEvent
{
    private const int _ADD_COIN_TEXT_ID = 105;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        CardData card = context.card;
        if (character == null || card == null) return;

        // ��D�ɓ����������邩����
        bool isSame = false;
        List<int> handList = character.possessCard.handCardIDList;
        int cardAdvance = card.advance;
        for (int i = 0, max = handList.Count; i < max; i++)
        {
            CardData handCard = CardManager.instance.GetCard(handList[i]);
            if (handCard.advance == cardAdvance) isSame = true;
        }

        if (!isSame) return;

        character.AddCoin(param);
        await UIManager.instance.RunMessage(string.Format(_ADD_COIN_TEXT_ID.ToText(), param));
    }
}
