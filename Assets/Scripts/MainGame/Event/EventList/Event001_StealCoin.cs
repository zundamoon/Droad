using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class Event001_StealCoin : BaseEvent
{
    private const int _STEAL_TEXT_ID = 115;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;
        // ƒvƒŒƒCƒ„[‚ÌˆÚ“®Œã‚Ìˆ—‚ÉÝ’è
        character.SetAfterMoveEvent(async (targetCharacterList) =>
        {
            int addCoin = 0;
            for (int i = 0, max = targetCharacterList.Count; i < max; i++)
            {
                addCoin += targetCharacterList[i].RemoveCoin(param);
            }
            if (addCoin <= 0) return;

            character.AddCoin(addCoin);
            await UIManager.instance.RunMessage(string.Format(_STEAL_TEXT_ID.ToText(), addCoin));
        });
        await UniTask.CompletedTask;
    }
}
