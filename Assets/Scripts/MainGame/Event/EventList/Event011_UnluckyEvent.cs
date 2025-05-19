using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event011_UnluckyEvent : BaseEvent
{
    /// <summary>
    /// イベントのIDリスト
    /// </summary>
    private int[] _eventIDList = { };

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        // イベントの抽選
        int eventID = DicideEvent();
        await EventManager.ExecuteEvent(eventID, context);
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
