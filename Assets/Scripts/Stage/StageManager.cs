using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    [SerializeField] public StageData stageData;

    public void Start()
    {
        instance = this;
    }

    /// <summary>
    /// ステージの座標からマスのイベントを取得
    /// </summary>
    /// <param name="stagePosition"></param>
    /// <returns></returns>
    public int GetSquareEvent(StagePosition stagePosition)
    {
        GameObject square = stageData.stageRoute.routeList[stagePosition.route].roadList[stagePosition.road].squareList[stagePosition.square];
        Square squareData = square.GetComponent<Square>();
        return squareData.GetEventID();
    }
    
    /// <summary>
    /// プレイヤーの座標から次のマスを返す
    /// </summary>
    /// <param name="playerPos"></param>
    /// <returns></returns>
    public GameObject CheckNextSqaure(StagePosition playerPos)
    {
        GameObject square;
        square = stageData.stageRoute.routeList[playerPos.route].roadList[playerPos.road].squareList[playerPos.square + 1];
        if (square != null) return square;
        return null;
    }
}
