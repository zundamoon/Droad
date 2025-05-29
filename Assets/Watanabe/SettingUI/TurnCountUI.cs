using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;

public class TurnCountUI : BaseCountUI
{
    public override void AddCount(int value)
    {
        count += value;
        if (count <= 0) count = TURN_MAX;
        if (count > TURN_MAX) count = 1;
        countText.text = count.ToString();
    }
}
