using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConst;

public class Event031_StealCoinEveryone : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // �S���R�C�������炷
        int stealCount = 0;
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            if (character.playerID == i) continue;
            Character targetCharacter = CharacterManager.instance.GetCharacter(i);
            stealCount += targetCharacter.RemoveCoin(param);
        }
        character.AddCoin(stealCount);

        await UniTask.CompletedTask;
    }
}
