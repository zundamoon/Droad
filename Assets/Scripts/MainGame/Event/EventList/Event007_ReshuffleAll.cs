using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConst;

public class Event007_ReshuffleAll : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(i);
            await character.possessCard.ReshuffleDeck();
        }
    }
}
