using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExpansionMethod
{
    public static string ToText(this int textID)
    {
        return TextMasterUtility.GetText(textID);
    }
}
