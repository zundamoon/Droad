using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event026_ReturnDiscardToDeck : BaseEvent
{
    private const int _CHOICE_TEXT_ID = 123;
    private const int _NO_DISCARD_TEXT_ID = 125;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        List<int> discardCards = character.possessCard.discardCardIDList;
        for (int i = 0; i < param; i++)
        {
            // 捨て札がないなら処理しない
            if (discardCards.Count <= 0)
            {
                await UIManager.instance.RunMessage(_NO_DISCARD_TEXT_ID.ToText());
                return;
            }

            // 選択の呼び出し
            await UIManager.instance.SetChoiceCallback((cardID) =>
            {
                // デッキに戻す
                character.possessCard.ReturnDiscardToDeck(cardID);
            });

            await UIManager.instance.RunMessage(_CHOICE_TEXT_ID.ToText());
            await UIManager.instance.OpenChoiceArea(discardCards);
        }
    }
}
