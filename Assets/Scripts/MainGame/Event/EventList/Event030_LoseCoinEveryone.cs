using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event030_LoseCoinEveryone : BaseEvent
{
    private const int _TEXT_ID = 139;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        // ‘SˆõƒRƒCƒ“‚ðŒ¸‚ç‚·
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            Character targetCharacter = CharacterManager.instance.GetCharacter(i);
            targetCharacter.RemoveCoin(param);
        }
        await UIManager.instance.RunMessage(string.Format(_TEXT_ID.ToText(), param));
    }
}
