using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event031_StealCoinEveryone : BaseEvent
{
    private const int _TEXT_ID = 140;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // ‘SˆõƒRƒCƒ“‚ðŒ¸‚ç‚·
        int stealCount = 0;
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            if (character.playerID == i) continue;
            Character targetCharacter = CharacterManager.instance.GetCharacter(i);
            stealCount += targetCharacter.RemoveCoin(param);
        }
        character.AddCoin(stealCount);
        await UIManager.instance.RunMessage(string.Format(_TEXT_ID.ToText(), stealCount));
    }
}
