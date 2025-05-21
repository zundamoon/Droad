using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class Event001_StealCoin : BaseEvent
{
    private Character _sourceCharacter = null;
    private int _stealCoinCount = 0;
    private const int _STEAL_TEXT_ID = 115;

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
    private async UniTask StealCoin(List<Character> targetCharacterList)
    {
        int addCoin = 0;
        for (int i = 0, max = targetCharacterList.Count; i < max; i++)
        {
            addCoin += targetCharacterList[i].RemoveCoin(_stealCoinCount);
        }
        _sourceCharacter.AddCoin(addCoin);
        await UIManager.instance.RunMessage(string.Format(_STEAL_TEXT_ID.ToText(), addCoin));
    }
}
