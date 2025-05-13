using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

using static Entity_EventData;

public class EventManager
{
    // �C�x���g�̃��X�g
    public static List<BaseEvent> eventList { get; private set; } = null;

    public static void Init()
    {
        eventList = new List<BaseEvent>();
        eventList.Add(new Event000_DiscardHand());
        eventList.Add(new Event001_StealCoin());
        eventList.Add(new Event002_Reshuffle());
        eventList.Add(new Event003_DiscardDeck());
        eventList.Add(new Event004_CancelNextSquareEvent());
        eventList.Add(new Event005_DiscardDeck1Exe());
        eventList.Add(new Event006_AddCoin());
    }

    /// <summary>
    /// �C�x���g�̎��s
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sourceCharacter"></param>
    /// <param name="eventID"></param>
    /// <param name="setParam"></param>
    public static async UniTask ExecuteEvent(Character sourceCharacter, int eventID, int addParam = -1)
    {
        Param eventMaster = EventMasterUtility.GetEventMaster(eventID);
        if (eventMaster == null) return;

        int eventIndex = eventMaster.eventType;
        int eventParam = eventMaster.param[0];
        await eventList[eventIndex].PlayEvent(sourceCharacter, eventParam, addParam);
    }
}
