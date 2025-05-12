using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class StageManager : SystemObject
{
    public static StageManager instance;
    [SerializeField] public StageData stageData;

    public void Awake()
    {
        instance = this;
    }

    private Square GetSquare(StagePosition squarePos)
    {
        GameObject squareObject = stageData.stageRoute.routeList[squarePos.m_route].roadList[squarePos.m_road].squareList[squarePos.m_square];
        Square square = squareObject.GetComponent<Square>();

        return square;
    }

    /// <summary>
    /// ステージの座標からマスのイベントを取得
    /// </summary>
    /// <param name="stagePosition"></param>
    /// <returns></returns>
    public int GetSquareEvent(StagePosition squarePos) { return GetSquare(squarePos).GetEventID(); }

    /// <summary>
    /// 一旦停止マスか確認
    /// </summary>
    /// <param name="squarePos"></param>
    /// <returns></returns>
    public bool CheckStopSquare(StagePosition squarePos) { return GetSquare(squarePos).isStopSquare; }

    /// <summary>
    /// 渡された座標の一マス先の座標を確認
    /// </summary>
    /// <param name="playerPos"></param>
    /// <returns></returns>
    //public StagePosition CheckNextPosition(StagePosition playerPos)
    //{
    //    StagePosition nextPos;
    //    nextPos = playerPos;
    //    nextPos.m_square = playerPos.m_square + 1;
    //    GameObject nextSquare;

    //    var roadList = stageData.stageRoute.routeList;
    //    nextSquare = stageData.stageRoute.routeList[playerPos.m_route].roadList[playerPos.m_road].squareList[nextPos.m_square];

    //    // 進めるマスがなくなっていなかったら座標を返す
    //    if (nextSquare != null) return nextPos;
    //    nextPos.m_route += 1;
    //    // 次のルートにいくつ道があるか取得
    //    int roadCount = stageData.stageRoute.routeList[nextPos.m_route].roadList.Count;
    //    nextPos.m_road = roadCount;
    //    nextPos.m_square = 0;

    //    return nextPos;
    //}

    public StagePosition CheckNextPosition(StagePosition playerPos)
    {
        StagePosition nextPos = playerPos;
        nextPos.m_square += 1;

        var routeList = stageData.stageRoute.routeList;

        // 次のマスに移動できるかチェック
        if (playerPos.m_route < routeList.Count)
        {
            var roadList = routeList[playerPos.m_route].roadList;

            if (playerPos.m_road < roadList.Count)
            {
                var squareList = roadList[playerPos.m_road].squareList;

                if (nextPos.m_square < squareList.Count)
                {
                    GameObject nextSquare = squareList[nextPos.m_square];
                    if (nextSquare != null)
                    {
                        return nextPos;
                    }
                }
            }
        }

        // 次の道に進む処理
        nextPos.m_route += 1;
        nextPos.m_road = 0;
        nextPos.m_square = 0;

        // 次の道でマス移動できるかをチェック
        if (nextPos.m_route < routeList.Count)
        {
            // 道リスト
            var nextRoadList = routeList[nextPos.m_route].roadList;

            if (nextRoadList.Count > 0)
            {
                // マス目
                var nextSquareList = nextRoadList[0].squareList;

                if (nextSquareList.Count > 0)
                {
                    GameObject nextSquare = nextSquareList[0];
                    if (nextSquare != null)
                    {
                        return nextPos;
                    }
                }
            }
        }

        nextPos.m_route = 1;
        nextPos.m_road = 0;
        nextPos.m_square = 0;

        return nextPos;
    }

    /// <summary>
    /// StagePositionからVector3に変換する
    /// </summary>
    /// <param name="squarePosition"></param>
    /// <returns></returns>
    public Vector3 GetPosition(StagePosition squarePosition)
    {
        GameObject squareObject = stageData.stageRoute.routeList[squarePosition.m_route].roadList[squarePosition.m_road].squareList[squarePosition.m_square];
        return squareObject.transform.position;
    }
}
