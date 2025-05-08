using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public List<BaseEvent> eventList { get; private set; } = null;

    public void Init()
    {
        eventList = new List<BaseEvent>();
        eventList.Add(new Event001_DiscardHand());
    }
}
