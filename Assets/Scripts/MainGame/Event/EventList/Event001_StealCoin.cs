using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Event001_StealCoin : BaseEvent
{
    private Character _sourceCharacter = null;
    private int _stealCoinCount = 0;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;
        // プレイヤーの移動後の処理に設定
        character.SetAfterMoveEvent(StealCoin);
        _sourceCharacter = character;
        _stealCoinCount = param;
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// コインを盗む
    /// </summary>
    /// <param name="sourceCharacter"></param>
    /// <param name="targetCharacterList"></param>
    /// <param name="stealCount"></param>
    private void StealCoin(List<Character> targetCharacterList)
    {
        int addCoin = 0;
        for (int i = 0, max = targetCharacterList.Count; i < max; i++)
        {
            addCoin += targetCharacterList[i].RemoveCoin(_stealCoinCount);
        }
        _sourceCharacter.AddCoin(addCoin);
    }
}
