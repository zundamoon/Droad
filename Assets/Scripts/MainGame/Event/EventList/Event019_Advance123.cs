using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Event019_Advance123 : BaseEvent
{
    private readonly List<int> _CHOICE_CARD_ID_LIST = new List<int>{ 0, 1, 2 };
    private const int _TEXT_ID = 121;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        // ‘I‘ð‚ÌŒÄ‚Ño‚µ
        await UIManager.instance.SetChoiceCallback((cardID) =>
        {
            CardData newCard = new CardData();
            newCard.SetAdvance(CardManager.instance.GetCard(cardID).advance);
            context.card = newCard;
        });

        await UIManager.instance.RunMessage(_TEXT_ID.ToText());
        await UIManager.instance.OpenChoiceArea(_CHOICE_CARD_ID_LIST);
    }
}
