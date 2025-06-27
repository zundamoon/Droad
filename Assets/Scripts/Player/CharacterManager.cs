using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

using static GameConst;
using System.Linq;
using System.Threading.Tasks;

public class CharacterManager : SystemObject
{
    [SerializeField] private GameObject characterObject = null;
    [SerializeField] private List<Color> _playerColorList = null;
    private List<Character> _characterList = null;
    public static CharacterManager instance = null;

    public override async UniTask Initialize()
    {
        instance = this;
        _characterList = new List<Character>(GameDataManager.instance.playerMax);

        // キャラクターの生成
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            GenerateCharacter(i);
        }
    }

    private void GenerateCharacter(int setID)
    {
        GameObject generatedObject = Instantiate(characterObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        Character character = generatedObject.GetComponent<Character>();
        Color passColor = _playerColorList[setID];
        character.SetPlayerColor(passColor);
        character.Init(setID);
        character.SetUpdateStatus(async (character) =>
        {
            await UIManager.instance.UpdateStatus(character);
            instance.UpdateRank();
        });
        _characterList.Add(character);
    }

    public Character GetCharacter(int characterID) { return _characterList[characterID]; }

    public List<Character> GetAllCharacter() {  return _characterList; }

    public Color GetCharacterColor(int playerID)
    {
        return _characterList[playerID].playerColor;
    }

    /// <summary>
    /// 順位を更新する
    /// </summary>
    public async Task UpdateRank()
    {
        // 順位でソート
        var rankList = _characterList.OrderByDescending(p => p.stars)
            .ThenByDescending(p => p.coins).ToList();

        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            // 同率判定
            if (i > 0 &&
                rankList[i].stars == rankList[i - 1].stars &&
                rankList[i].coins == rankList[i - 1].coins
                )
                rankList[i].SetRank(rankList[i - 1].rank);
            else
                rankList[i].SetRank(i + 1);
        }

        // プレイヤーに反映
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            var rankPlayer = rankList.First(p => p.playerID == _characterList[i].playerID);
            if (rankPlayer == null) return;

            _characterList[i].SetRank(rankPlayer.rank);
            await UIManager.instance.UpdateStatus(_characterList[i]);
        }
    }

    /// <summary>
    /// ランクリストを取得
    /// </summary>
    /// <returns></returns>
    public List<List<int>> GetRankList()
    {
        List<List<int>> rank = new List<List<int>>();

        // 各プレイヤー分、空のList<int>を用意
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            rank.Add(new List<int>());
        }

        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            for (int j = 0; j < GameDataManager.instance.playerMax; j++)
            {
                if (_characterList[j].rank != i + 1) continue;

                int coin = _characterList[j].coins;
                int star = _characterList[j].stars;

                // i番目の順位リストにデータを追加
                rank[i].Add(j);     // プレイヤーID or インデックス
                rank[i].Add(coin);  // コイン数
                rank[i].Add(star);  // スター数
            }
        }

        return rank;

    }

    /// <summary>
    /// 1位のプレイヤー取得
    /// </summary>
    /// <returns></returns>
    public Character GetTopPlayer()
    {
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            if (_characterList[i].rank != 1) continue;
            return _characterList[i];
        }
        return null;
    }
}
