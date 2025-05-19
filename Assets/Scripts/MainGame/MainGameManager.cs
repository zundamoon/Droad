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
    private const int _TURN_TEXT_ID = 100;

    public override async UniTask Initialize()
    {
        Application.targetFrameRate = 60;

        MasterDataManager.LoadAllData();
        EventManager.Init();
        CardManager.Init();
        CameraManager.Init();

        _turnProcessor = new TurnProcessor();
        _turnProcessor.Init();
        // カードのコールバックを設定
        CardObject.SetOnUseCard(_turnProcessor.AcceptCard);
        await CameraManager.SetAnchor(StageManager.instance.GetCameraAnchor(), 0);
        await MainGameProc();
    }

    /// <summary>
    /// メインゲームの処理
    /// </summary>
    private async UniTask MainGameProc()
    {
        while (currentTurn < _TURN_MAX)
        {
            await TurnCountUp();
            await _turnProcessor.TurnProc();
        }
    }
    
    /// <summary>
    /// ターンをカウントアップ
    /// </summary>
    private async UniTask TurnCountUp()
    {
        currentTurn++;
        // UIに通知
        await UIManager.instance.RunMessage(string.Format(_TURN_TEXT_ID.ToText(), currentTurn, _TURN_MAX));
        await UIManager.instance.AddStatus(string.Format(_TURN_TEXT_ID.ToText(), currentTurn + 1, _TURN_MAX));
    }
}
