using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Entity_TextData;

public class TextMasterUtility
{
    public static Param GetTextMaster(int ID)
    {
        List<Param> textMasterList = MasterDataManager.textData[0];
        for (int i = 0, max = textMasterList.Count; i < max; i++)
        {
            if (textMasterList[i].ID != ID) continue;

            return textMasterList[i];
        }
        return null;
    }

    public static string GetText(int ID)
    {
        if (ID < 0) return "";

        return GetTextMaster(ID).text;
    }
}
