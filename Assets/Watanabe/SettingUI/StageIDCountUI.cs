using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIDCountUI : BaseCountUI
{
    public override void AddCount(int value)
    {
        count += value;
        if (count <= 0) count = 0;
        countText.text = count.ToString();
    }
}
