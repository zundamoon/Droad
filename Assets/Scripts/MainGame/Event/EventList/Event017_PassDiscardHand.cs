using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event017_PassDiscardHand : BaseEvent
{
    private const int _TEXT_ID = 119;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // ƒvƒŒƒCƒ„[‚ÌˆÚ“®Œã‚Ìˆ—‚Éİ’è
        character.SetAfterMoveEvent(async (targetCharacterList) =>
        {
            int targetCount = targetCharacterList.Count;
            if (targetCount <= 0) return;

            for (int i = 0; i < targetCount; i++)
            {
                // èD‚ğÌ‚Ä‚é
                targetCharacterList[i].possessCard.DiscardRandHand(param);
            }
            await UIManager.instance.RunMessage(string.Format(_TEXT_ID.ToText(), param));
        });
        await UniTask.CompletedTask;
    }
}
