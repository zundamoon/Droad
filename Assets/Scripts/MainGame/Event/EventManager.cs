using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

using static Entity_EventData;

public class EventManager
{
    // イベントのリスト
    public static List<IEvent> eventList { get; private set; } = null;

    public static void Init()
    {
        eventList = new List<IEvent>();
        eventList.Add(new Event000_DiscardHand());
        eventList.Add(new Event001_StealCoin());
        eventList.Add(new Event002_Reshuffle());
        eventList.Add(new Event003_DiscardDeck());
        eventList.Add(new Event004_CancelNextSquareEvent());
        eventList.Add(new Event005_DiscardDeck1Exe());
        eventList.Add(new Event006_AddCoin());
        eventList.Add(new Event007_ReshuffleAll());
        eventList.Add(new Event008_LoseCoin());
        eventList.Add(new Event009_TurningRoute());
        eventList.Add(new Event010_LuckyEvent());
        eventList.Add(new Event011_UnluckyEvent());
    }

    /// <summary>
    /// イベントの実行
    /// </summary>
    /// <param name="eventID"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async UniTask ExecuteEvent(int eventID, EventContext context = null)
    {
        Param eventMaster = EventMasterUtility.GetEventMaster(eventID);
        if (eventMaster == null) return;

        int eventIndex = eventMaster.eventType;
        int eventParam = eventMaster.param[0];
        await eventList[eventIndex].ExecuteEvent(context, eventParam);
    }
}
