using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static GameEnum;

public class SquareManager : SystemObject
{
    SquareManager instance = null;
    public override async UniTask Initialize()
    {
        instance = this;
    }

    public void SquareDefaultSetting()
    {

    }

}
