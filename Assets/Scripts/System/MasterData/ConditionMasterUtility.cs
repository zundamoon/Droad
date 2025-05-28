using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Entity_ConditionData;

public class ConditionMasterUtility
{
    public static Param GetConditionMaster(int ID)
    {
        List<Param> conditionMasterList = MasterDataManager.conditionData[0];
        for (int i = 0, max = conditionMasterList.Count; i < max; i++)
        {
            if (conditionMasterList[i].ID != ID) continue;

            return conditionMasterList[i];
        }
        return null;
    }
}
