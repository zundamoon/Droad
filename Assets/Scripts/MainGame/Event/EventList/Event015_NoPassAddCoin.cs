using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event015_NoPassAddCoin : BaseEvent
{
    private const int _TEXT_ID = 118;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // ƒvƒŒƒCƒ„[‚ÌˆÚ“®Œã‚Ìˆ—‚ÉÝ’è
        character.SetAfterMoveEvent(async (targetCharacterList) =>
        {
            if (targetCharacterList.Count > 0) return;

            character.AddCoin(param);
            await UIManager.instance.RunMessage(string.Format(_TEXT_ID.ToText(), param));
        });
        await UniTask.CompletedTask;
    }
}
