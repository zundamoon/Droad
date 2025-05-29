using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameData")]
public class GameData : ScriptableObject
{
    public List<int> rankList = null;
    public int settingStageID = -1;
    public int settingPlayerCount = -1;
    public int settingTurnCount = -1;
}
