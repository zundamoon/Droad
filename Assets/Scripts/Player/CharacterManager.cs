using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

using static GameConst;
using UnityEngine.Rendering;

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

        // キャラクターの生成
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
}
