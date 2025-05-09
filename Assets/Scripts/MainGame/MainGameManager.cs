using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : SystemObject
{
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
    /// ���C���Q�[���̏���
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
    /// �^�[�����J�E���g�A�b�v
    /// </summary>
    private void TurnCountUp()
    {
        currentTurn++;
        // UI�ɒʒm
    }
}
