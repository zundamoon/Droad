using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;

public class PlayerCountUI : BaseCountUI
{
    public override void AddCount(int value)
    {
        count += value;
        if (count <= 0) count = PLAYER_MAX;
        if (count > PLAYER_MAX) count = 1;
        countText.text = count.ToString();
    }
}
