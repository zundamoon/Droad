using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

using static CommonModule;
using static GameConst;

public class TurnProcessor
{
    private List<Character> _playerList = null;
    private List<int> _playerOrder = null;

    public void Init()
    {
        _playerList = new List<Character>(_PLAYER_MAX);
        _playerOrder = new List<int>(_PLAYER_MAX);

        CharacterManager characterManager = CharacterManager.instance;

        for (int i = 0; i < _PLAYER_MAX; i++)
        {
            characterManager.GenerateCharacter();
        }

        _playerList = characterManager.GetCharacterList();
    }

    /// <summary>
    /// 各ターンの処理
    /// </summary>
    public async UniTask TurnProc()
    {
        // 手番決め
        await DesidePlayerOrder();

        // 各手番
        for (int i = 0; i < _PLAYER_MAX; i++)
        {
            int orderIndex = _playerOrder[i];
            await EachTurn(_playerList[orderIndex]);
        }
    }

    /// <summary>
    /// 手番を決める
    /// </summary>
    private async UniTask DesidePlayerOrder()
    {
        _playerOrder.Clear();
        List<int> playCardList = new List<int>(_PLAYER_MAX);
        for (int i = 0; i < _PLAYER_MAX; i++)
        {
            // 手札の選択
            int playCardCount = 0;
            playCardList.Add(playCardCount);
        }
        // 出されたカードから順番を決める
        while (playCardList.Count > 0)
        {
            // 最大値のインデックスを取得
            int max = playCardList.Max();
            List<int> indexList = new List<int>();
            for (int j = 0; j < _PLAYER_MAX; j++)
            {
                if (indexList[j] != max) continue;
                indexList.Add(j);
                playCardList.RemoveAt(j);
            }
            // 複数ならランダムに決定
            while (indexList.Count > 0)
            {
                int index = Random.Range(0, indexList.Count);
                _playerOrder.Add(indexList[index]);
                indexList.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// キャラクターのターン処理
    /// </summary>
    /// <param name="turnCharacter"></param>
    private async UniTask EachTurn(Character turnCharacter)
    {
        if (turnCharacter == null) return;

        StageManager stageManager = StageManager.instance;
        // カードのIDから進む回数を取得
        int advanceValue = 2;
        // 進む分だけ動く
        // 動く途中のキャラクターを保持
        List<Character> targetCharacterList = new List<Character>(_PLAYER_MAX);
        for (int i = 0; i < advanceValue; i++)
        {
            // 移動先のマスを取得
            StagePosition nextPosition = stageManager.CheckNextPosition(turnCharacter.position);
            Vector3 movePosition = stageManager.GetPosition(nextPosition);

            // 移動
            await turnCharacter.Move(movePosition);
            turnCharacter.position = nextPosition;
            // マス上にいる他キャラを検出
            for (int j = 0; j < _PLAYER_MAX; j++)
            {
                Character target = _playerList[j];

                if (target == turnCharacter) continue;
                if (target.position == nextPosition) targetCharacterList.Add(target);
            }

            // 停止マスでなければ次へ
            if (!stageManager.CheckStopSquare(turnCharacter.position)) continue;
            // イベント可能ならその場で終了
            if (!turnCharacter.CanEvent()) return;

            ExcuteSquareEvent(turnCharacter);
        }

        await UniTask.DelayFrame(1000);

        // 移動後処理を実行
        turnCharacter.ExecuteAfterMoveEvent(targetCharacterList);

        // イベント可能でなければ終了
        if (!turnCharacter.CanEvent()) return;
        ExcuteSquareEvent(turnCharacter);
    }

    private void ExcuteSquareEvent(Character target)
    {
        int squareEventID = StageManager.instance.GetSquareEvent(target.position);
        EventManager.ExecuteEvent(target, squareEventID);
    }
}
