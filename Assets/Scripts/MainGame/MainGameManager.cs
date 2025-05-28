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

    private const int _TURN_MAX = 30;
    private const int _TURN_TEXT_ID = 100;
    private const int _END_GAME_TEXT_ID = 128;
    private const int _WIN_PLAYER_TEXT_ID = 129;
    private const string _RESULT_SCENE_NAME = "ResultScene";

    public override async UniTask Initialize()
    {
        Application.targetFrameRate = 60;

        _turnProcessor = new TurnProcessor();
        _turnProcessor.Init();
        await CameraManager.SetAnchor(StageManager.instance.GetCameraAnchor(), 0);

        await MainGameProc();
    }

    /// <summary>
    /// メインゲームの処理
    /// </summary>
    private async UniTask MainGameProc()
    {
        StageManager.instance.DecideStarSquare();
        while (currentTurn < _TURN_MAX)
        {
            await TurnCountUp();
            await _turnProcessor.TurnProc();
        }
        // ゲーム終了処理
        await EndGame();
    }

    /// <summary>
    /// ターンをカウントアップ
    /// </summary>
    private async UniTask TurnCountUp()
    {
        currentTurn++;
        // UIに通知
        await UIManager.instance.RunMessage(string.Format(_TURN_TEXT_ID.ToText(), currentTurn, _TURN_MAX));
        await UIManager.instance.AddStatus(string.Format(_TURN_TEXT_ID.ToText(), currentTurn, _TURN_MAX));
    }

    /// <summary>
    /// ゲーム終了の処理
    /// </summary>
    /// <returns></returns>
    private async UniTask EndGame()
    {
        // 順位の表示
        await UIManager.instance.RunMessage(_END_GAME_TEXT_ID.ToText());
        Character top = CharacterManager.instance.GetTopPlayer();
        //await UIManager.instance.RunMessage(string.Format(_WIN_PLAYER_TEXT_ID.ToText()), top.playerID + 1);
        // ランクを保持
        SendData.rankList = CharacterManager.instance.GetRankList();
        // シーン遷移
        FadeSceneChange.ChangeSceneEvent(_RESULT_SCENE_NAME);
    }
}
