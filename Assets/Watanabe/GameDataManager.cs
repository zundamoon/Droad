using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameDataManager : SystemObject
{
    [SerializeField] public GameData gameData;
    public static GameDataManager instance;
    public int turnMax;
    public int playerMax;

    public override async UniTask Initialize()
    {
        instance = this;
        turnMax = GameDataManager.instance.gameData.settingTurnCount;
        playerMax = GameDataManager.instance.gameData.settingPlayerCount;
    }
}