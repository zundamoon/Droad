using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConst;

public class Event030_LoseCoinEveryone : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        // �S���R�C�������炷
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            Character targetCharacter = CharacterManager.instance.GetCharacter(i);
            targetCharacter.RemoveCoin(param);
        }
        await UniTask.CompletedTask;
    }
}
