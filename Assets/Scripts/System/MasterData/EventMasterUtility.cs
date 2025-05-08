using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Entity_EventData;

public class EventMasterUtility
{
    public static Param GetEventMaster(int ID)
    {
        List<Param> eventMasterList = MasterDataManager.eventData[0];
        for (int i = 0, max = eventMasterList.Count; i < max; i++)
        {
            if (eventMasterList[i].ID != ID) continue;

            return eventMasterList[i];
        }
        return null;
    }
}
