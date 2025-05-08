using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : SystemObject
{
    private TurnProcessor _turnProcessor = null;

    public override void Initialize()
    {
        MasterDataManager.LoadAllData();
        EventManager.Init();
        CardManager.Init();

        _turnProcessor = new TurnProcessor();
        _turnProcessor.Init();
    }
}
