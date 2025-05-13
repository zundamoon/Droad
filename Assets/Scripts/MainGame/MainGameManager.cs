using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : SystemObject
{
    [SerializeField]
    public List<Character> characterList { get; private set; } = null;

    private TurnProcessor _turnProcessor = null;

    public int currentTurn { get; private set; } = 0;

    private const int _TURN_MAX = 15;

    public override void Initialize()
    {
        MasterDataManager.LoadAllData();
        EventManager.Init();
        CardManager.Init();

        _turnProcessor = new TurnProcessor();
        _turnProcessor.Init();

        MainGameProc();
    }

    /// <summary>
    /// メインゲームの処理
    /// </summary>
    private void MainGameProc()
    {
        while (currentTurn < _TURN_MAX)
        {
            TurnCountUp();
            _turnProcessor.TurnProc();
        }
    }
    
    /// <summary>
    /// ターンをカウントアップ
    /// </summary>
    private void TurnCountUp()
    {
        currentTurn++;
        // UIに通知
    }
}
