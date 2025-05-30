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

        // プレイヤーの移動後の処理に設定
        character.SetAfterMoveEvent(async (targetCharacterList) =>
        {
            int targetCount = targetCharacterList.Count;
            if (targetCount <= 0) return;

            for (int i = 0; i < targetCount; i++)
            {
                // 手札を捨てる
                targetCharacterList[i].possessCard.DiscardRandHand(param);
            }
            await UIManager.instance.RunMessage(string.Format(_TEXT_ID.ToText(), param));
        });
        await UniTask.CompletedTask;
    }
}
