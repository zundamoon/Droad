using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event002_StealCoin : BaseEvent
{
    private Character _sourceCharacter = null;
    private int _stealCoinCount = 0;

    public override void PlayEvent(Character sourceCharacter, int param, int addParam = -1)
    {
        // プレイヤーの移動後の処理に設定
        sourceCharacter.SetAfterMoveEvent(StealCoin);
        _sourceCharacter = sourceCharacter;
        _stealCoinCount = param;
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
