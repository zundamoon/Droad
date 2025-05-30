using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event007_ReshuffleAll : BaseEvent
{
    private const int _RESHUFFLE_ALL_TEXT_ID = 133;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        await UIManager.instance.RunMessage(_RESHUFFLE_ALL_TEXT_ID.ToText());
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(i);
            await character.possessCard.ReshuffleDeck();
        }
    }
}
