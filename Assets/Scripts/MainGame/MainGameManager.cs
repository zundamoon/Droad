using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MainGameManager : SystemObject
{
    [SerializeField]
    public List<Character> characterList { get; private set; } = null;

    private TurnProcessor _turnProcessor = null;

    public int currentTurn { get; private set; } = 0;

    private const int _TURN_MAX = 15;

    public override async UniTask Initialize()
    {
        MasterDataManager.LoadAllData();
        EventManager.Init();
        CardManager.Init();

        _turnProcessor = new TurnProcessor();
        _turnProcessor.Init();
        // �J�[�h�̃R�[���o�b�N��ݒ�
        CardObject.SetOnUseCard(_turnProcessor.AcceptCard);

        await MainGameProc();
    }

    /// <summary>
    /// ���C���Q�[���̏���
    /// </summary>
    private async UniTask MainGameProc()
    {
        while (currentTurn < _TURN_MAX)
        {
            TurnCountUp();
            await _turnProcessor.TurnProc();
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
