using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;

public class SquareManager : SystemObject
{
    SquareManager instance = null;
    public override void Initialize()
    {
        instance = this;
    }

    public void SquareDefaultSetting()
    {

    }

}
