using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;

public class CharacterManager : SystemObject
{
    [SerializeField] private GameObject characterObject = null;
    public List<Character> characterList = null;
    public static CharacterManager instance = null;

    public override void Initialize()
    {
        instance = this;
        characterList = new List<Character>();
    }

    public void GenerateCharacter()
    {
        GameObject generatedObject = Instantiate(characterObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        Character character = generatedObject.GetComponent<Character>();
        characterList.Add(character);
    }

    public List<Character> GetCharacterList() { return characterList; }
}
