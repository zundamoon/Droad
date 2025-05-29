using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConst;

public class Event030_LoseCoinEveryone : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        // ‘SˆõƒRƒCƒ“‚ðŒ¸‚ç‚·
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            Character targetCharacter = CharacterManager.instance.GetCharacter(i);
            targetCharacter.RemoveCoin(param);
        }
        await UniTask.CompletedTask;
    }
}
