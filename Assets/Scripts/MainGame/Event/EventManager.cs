using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Entity_EventData;

public class EventManager
{
    // イベントのリスト
    public static List<BaseEvent> eventList { get; private set; } = null;

    public static void Init()
    {
        eventList = new List<BaseEvent>();
        eventList.Add(new Event001_DiscardHand());
    }

    /// <summary>
    /// イベントの実行
    /// </summary>
    /// <param name="sourceCharacter"></param>
    /// <param name="eventID"></param>
    public static void ExecuteEvent(Character sourceCharacter, int eventID)
    {
        Param eventMaster = EventMasterUtility.GetEventMaster(eventID);
        if (eventMaster != null) return;

        int eventIndex = eventMaster.eventType;
        int eventParam = eventMaster.param[0];
        eventList[eventIndex].PlayEvent(sourceCharacter, eventParam);
    }
}
