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
        eventList.Add(new Event001_PassStealCoin());
        eventList.Add(new Event002_Reshuffle());
        eventList.Add(new Event003_DiscardDeckTop());
        eventList.Add(new Event004_CancelNextSquareEvent());
        eventList.Add(new Event005_DiscardDeckTopExe());
        eventList.Add(new Event006_AddCoin());
        eventList.Add(new Event007_ReshuffleAll());
        eventList.Add(new Event008_LoseCoin());
        eventList.Add(new Event009_TurningRoute());
        eventList.Add(new Event010_LuckyEvent());
        eventList.Add(new Event011_UnluckyEvent());
        eventList.Add(new Event012_BuyStar());
        eventList.Add(new Event013_Shop());
        eventList.Add(new Event014_DiscardHandAll());
        eventList.Add(new Event015_NoPassAddCoin());
        eventList.Add(new Event016_RepeatNextSquareEvent());
        eventList.Add(new Event017_PassDiscardHand());
        eventList.Add(new Event018_PassStealStar());
        eventList.Add(new Event019_Advance123());
        eventList.Add(new Event020_Advance456());
        eventList.Add(new Event021_PassExchangeHand());
        eventList.Add(new Event022_DiscardHandAdvance());
        eventList.Add(new Event023_ExecuteDiscardCard());
        eventList.Add(new Event024_ExecuteHandCard());
        eventList.Add(new Event025_SameAdvanceAddCoin());
        eventList.Add(new Event026_ReturnDiscardToDeck());

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
