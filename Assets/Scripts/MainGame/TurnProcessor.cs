using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnProcessor
{
    private List<Character> _playerList = null;

    private const int _PLAYER_MAX = 4;

    public void Init()
    {
        _playerList = new List<Character>(_PLAYER_MAX);
    }

    public void TurnProc()
    {
        // 手番決め
        DesidePlayerOrder();

        // 各手番
        for (int i = 0; i < _PLAYER_MAX; i++)
        {
            EachTurn(_playerList[i]);
        }
    }

    /// <summary>
    /// 手番を決める
    /// </summary>
    private void DesidePlayerOrder()
    {
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
                _playerList.Add(indexList[index]);
                indexList.RemoveAt(index);
            }
        }
    }

    private void EachTurn(Character turnCharacter)
    {
        // 手札を選ぶ


        // 移動する対象
        Character character = turnCharacter;
        // カードのIDから進む回数を取得
        int advanceValue = 6;
        // 進む分だけ動く
        for (int i = 0; i < advanceValue; i++)
        {
            // 進行中の道に次のマスがあるか確認
            if (StageManager.instance.CheckNextSqaure(character.position) == null)
            {
                // 分岐があれば道を選択（選択されるまで動かない）

            }
            // 移動
            // character.Move();
        }
    }
}
