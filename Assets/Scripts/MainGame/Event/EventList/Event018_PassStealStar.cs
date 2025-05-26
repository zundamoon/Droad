using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event018_PassStealStar : BaseEvent
{
    private const int _TEXT_ID = 120;

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

            int stealCount = 0;
            for (int i = 0; i < targetCount; i++)
            {
                for (int j = 0; j < param; j++)
                {
                    // スターカードを奪う
                    int cardID = targetCharacterList[i].possessCard.RemoveRandomStarCard();
                    if (cardID <= 0) continue;

                    stealCount++;
                    await character.possessCard.AddCardDiscard(cardID);
                }
            }
            await UIManager.instance.RunMessage(string.Format(_TEXT_ID.ToText(), stealCount));
        });
    }
}
