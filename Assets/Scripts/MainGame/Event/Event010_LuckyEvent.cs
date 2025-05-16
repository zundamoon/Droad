using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event010_LuckyEvent : BaseEvent
{
    /// <summary>
    /// イベントのIDリスト
    /// </summary>
    private int[] _eventIDList = {  };

    public override async UniTask PlayEvent(Character sourceCharacter, int param, Square square = null)
    {
        // イベントの抽選
        int eventID = DicideEvent();
        await EventManager.ExecuteEvent(sourceCharacter, eventID, square);
    }

    /// <summary>
    /// イベントIDを決める
    /// </summary>
    /// <returns></returns>
    public int DicideEvent()
    {
        int index = Random.Range(0, _eventIDList.Length);
        return _eventIDList[index];
    }
}
