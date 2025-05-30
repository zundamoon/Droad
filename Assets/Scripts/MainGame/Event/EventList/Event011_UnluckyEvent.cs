using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event011_UnluckyEvent : BaseEvent
{
    /// <summary>
    /// イベントIDと抽選割合
    /// </summary>
    private static Dictionary<int, int> eventWeights = new Dictionary<int, int>
    {
        { 33, 10 },
        { 35, 20 },
    };

    private const int _UNLUCKEY_TEXT_ID = 111;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        await UIManager.instance.RunMessage(_UNLUCKEY_TEXT_ID.ToText());
        // イベントの抽選
        int eventID = DicideEventID();
        await EventManager.ExecuteEvent(eventID, context);
    }

    /// <summary>
    /// イベントIDを決める
    /// </summary>
    /// <returns></returns>
    private int DicideEventID()
    {
        // 重みを取得
        int totalRatio = 0;
        foreach (var weight in eventWeights.Values)
        {
            totalRatio += weight;
        }
        int randomValue = Random.Range(0, totalRatio);

        // 重みからレアリティを選出
        int currentRatio = 0;
        foreach (var pair in eventWeights)
        {
            currentRatio += pair.Value;
            if (randomValue < currentRatio)
            {
                return pair.Key;
            }
        }
        return -1;
    }
}
