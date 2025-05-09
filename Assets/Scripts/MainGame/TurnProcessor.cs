using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TurnProcessor
{
    private List<Character> _playerList = null;
    private List<int> _playerOrder = null;

    private const int _PLAYER_MAX = 4;

    public void Init()
    {
        _playerList = new List<Character>(_PLAYER_MAX);
        _playerOrder = new List<int>(_PLAYER_MAX);
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
        // 手札を選ぶ
        int cardID = -1;
        // カードを取得
        Card useCard = CardManager.GetCard(cardID);
        if (useCard == null) return;
        // コインを増やす
        turnCharacter.AddCoin(useCard.addCoin);
        // カードのIDから進む回数を取得
        int advanceValue = useCard.advance;
        // 進む分だけ動く
        // 動く途中のキャラクターを保持
        List<Character> targetCharacterList = new List<Character>(_PLAYER_MAX);
        for (int i = 0; i < advanceValue; i++)
        {
            // 移動
            // character.Move();
            // 現在のマスを取得
            // マスからキャラクターを取得
            //targetCharacter.Add();
            // 現在のマスが停止マスか判定
            if (false) continue;

            // キャラクターのマスからイベントを取得し実行
            if (!turnCharacter.CanEvent()) return;

        }
        // 移動後処理
        turnCharacter.ExecuteAfterMoveEvent(targetCharacterList);

        // キャラクターのマスからイベントを取得し実行
        if (!turnCharacter.CanEvent()) return;
        
    }
}
