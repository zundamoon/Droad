using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConst;

public class Event007_ReshuffleAll : BaseEvent
{
    public override async UniTask PlayEvent(Character sourceCharacter, int param, Square square = null)
    {
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(i);
            await character.possessCard.ReshuffleDeck();
        }
    }
}
