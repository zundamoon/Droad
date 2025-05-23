using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

using static GameConst;
using UnityEngine.Rendering;
using System.Linq;

public class CharacterManager : SystemObject
{
    [SerializeField] private GameObject characterObject = null;
    [SerializeField] private List<Color> _playerColorList = null;
    private List<Character> _characterList = null;
    public static CharacterManager instance = null;

    public override async UniTask Initialize()
    {
        instance = this;
        _characterList = new List<Character>(PLAYER_MAX);

        // �L�����N�^�[�̐���
        for (int i = 0; i < PLAYER_MAX; i++)
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
        _characterList.Add(character);
    }

    public Character GetCharacter(int characterID) { return _characterList[characterID]; }

    public List<Character> GetAllCharacter() {  return _characterList; }

    public Color GetCharacterColor(int playerID)
    {
        return _characterList[playerID].playerColor;
    }

    /// <summary>
    /// ���ʂ��X�V����
    /// </summary>
    public void UpdateRank()
    {
        // ���ʂŃ\�[�g
        var rankList = _characterList.OrderByDescending(p => p.stars)
            .ThenByDescending(p => p.coins).ToList();

        for (int i = 0; i < PLAYER_MAX; i++)
        {
            // ��������
            if (i > 0 &&
                rankList[i].stars == rankList[i - 1].stars &&
                rankList[i].coins == rankList[i - 1].coins
                )
                rankList[i].SetRank(rankList[i - 1].rank);
            else
                rankList[i].SetRank(i + 1);
        }

        // �v���C���[�ɔ��f
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            var rankPlayer = rankList.First(p => p.playerID == _characterList[i].playerID);
            if (rankPlayer == null) return;

            _characterList[i].SetRank(rankPlayer.rank);
        }
    }
}
