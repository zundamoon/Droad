using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MainGameManager : SystemObject
{
    public static MainGameManager instance;
    private TurnProcessor _turnProcessor = null;

    public int currentTurn { get; private set; } = 0;

    private const int _TURN_TEXT_ID = 100;
    private const int _END_GAME_TEXT_ID = 128;
    private const string _RESULT_SCENE_NAME = "ResultScene";

    public override async UniTask Initialize()
    {
        instance = this;

        Application.targetFrameRate = 60;

        _turnProcessor = new TurnProcessor();
        _turnProcessor.Init();
        await CameraManager.SetAnchor(StageManager.instance.GetCameraAnchor(), 0);

        await MainGameProc();
    }

    /// <summary>
    /// ���C���Q�[���̏���
    /// </summary>
    private async UniTask MainGameProc()
    {
        StageManager.instance.DecideStarSquare();
        while (currentTurn < GameDataManager.instance.turnMax)
        {
            await TurnCountUp();
            await _turnProcessor.TurnProc();
        }
        // �Q�[���I������
        await EndGame();
    }

    /// <summary>
    /// �^�[�����J�E���g�A�b�v
    /// </summary>
    private async UniTask TurnCountUp()
    {
        currentTurn++;
        // UI�ɒʒm
        await UIManager.instance.RunMessage(string.Format(_TURN_TEXT_ID.ToText(), currentTurn, GameDataManager.instance.turnMax));
        await UIManager.instance.AddStatus(string.Format(_TURN_TEXT_ID.ToText(), currentTurn, GameDataManager.instance.turnMax));
    }

    /// <summary>
    /// �Q�[���I���̏���
    /// </summary>
    /// <returns></returns>
    private async UniTask EndGame()
    {
        // ���ʂ̕\��
        await UIManager.instance.RunMessage(_END_GAME_TEXT_ID.ToText());
        Character top = CharacterManager.instance.GetTopPlayer();
        //await UIManager.instance.RunMessage(string.Format(_WIN_PLAYER_TEXT_ID.ToText()), top.playerID + 1);
        // �����N��ێ�
        SendData.rankList = CharacterManager.instance.GetRankList();
        // �V�[���J��
        FadeSceneChange.ChangeSceneEvent(_RESULT_SCENE_NAME);
    }
}
