using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static CommonModule;
using static GameEnum;

public class StageManager : SystemObject
{
    public static StageManager instance;
    public GameObject stagePrefab;
    private StageData stageData;

    [SerializeField]
    private Transform _cameraAnchor = null;

    public override async UniTask Initialize()
    {
        instance = this;
        GenerateStage();
        GameObject stageDataObject = GameObject.Find("StageData");
        stageData = stageDataObject.GetComponent<StageData>();

        AllSquareInit();
    }

    public void GenerateStage()
    {
        Instantiate(stagePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
    }

    public void AllSquareInit()
    {
        int routeIndex = 0;
        // ���񂿁I
        // unnti
        foreach (var roadList in stageData.stageRoute.routeList)
        {
            int roadIndex = 0;
            foreach (var squareList in roadList.roadList)
            {
                int squareIndex = 0;
                foreach (var squareObject in squareList.squareList)
                {
                    if (squareObject != null)
                    {
                        Square square = GetSquare(squareObject);
                        square.Init(CheckNextPosition(square.GetSquarePosition()));
                    }
                    squareIndex++;
                }
                roadIndex++;
            }
            routeIndex++;
        }
    }

    public void ChangeSquareType(StagePosition pos, SquareType type)
    {
        Square square = GetSquare(pos);
        BaseSquareData newData = CreateSquareData(type);

        if (newData != null)
        {
            square.ChangeSquareType(newData);
        }
    }

    private BaseSquareData CreateSquareData(SquareType type)
    {
        switch (type)
        {
            case SquareType.BLUE: return new BlueSquare();
            case SquareType.RED: return new RedSquare();
            case SquareType.HAPPENING: return new HappeningSquare();
            case SquareType.SHOP: return new ShopSquare();
            case SquareType.INVALID: return new InvalidSquare();
            default: return null;
        }
    }

    private Square GetSquare(StagePosition squarePos)
    {
        GameObject squareObject = stageData.stageRoute.routeList[squarePos.m_route].roadList[squarePos.m_road].squareList[squarePos.m_square];
        Square square = squareObject.GetComponent<Square>();

        return square;
    }

    private Square GetSquare(GameObject squareObject)
    {
        Square square = squareObject.GetComponent<Square>();

        return square;
    }

    /// <summary>
    /// �X�e�[�W�̍��W����}�X�̃C�x���g���擾
    /// </summary>
    /// <param name="stagePosition"></param>
    /// <returns></returns>
    public int GetSquareEvent(StagePosition squarePos) { return GetSquare(squarePos).GetEventID(); }

    /// <summary>
    /// ��U��~�}�X���m�F
    /// </summary>
    /// <param name="squarePos"></param>
    /// <returns></returns>
    public bool CheckStopSquare(StagePosition squarePos) { return GetSquare(squarePos).GetIsStarSquare(); }

    //public StagePosition CheckNextPosition(StagePosition playerPos)
    //{
    //    StagePosition nextPos = playerPos;
    //    nextPos.m_square++;

    //    if (IsValidSquare(nextPos)) return nextPos;

    //    nextPos.m_route++;
    //    nextPos.m_road = 0;
    //    nextPos.m_square = 0;

    //    if (IsValidSquare(nextPos)) return nextPos;


    //    return new StagePosition
    //    {
    //        m_route = 1,
    //        m_road = 0,
    //        m_square = 0
    //    };
    //}

    public StagePosition GetNextPosition(StagePosition playerPos)
    {
        return GetSquare(playerPos).nextPositionList[0];
    }

    public List<StagePosition> CheckNextPosition(StagePosition playerPos)
    {
        List<StagePosition> nextPositions = new List<StagePosition>();

        // 現在の位置の次のマス
        StagePosition nextSquare = new StagePosition(playerPos.m_route, playerPos.m_road, playerPos.m_square + 1);
        if (IsValidSquare(nextSquare))
        {
            nextPositions.Add(nextSquare);
            return nextPositions;
        }

        // 同じroute内の分岐
        var roadList = stageData.stageRoute.routeList[playerPos.m_route].roadList;
        for (int i = 0; i < roadList.Count; i++)
        {
            if (i == playerPos.m_road) continue;

            StagePosition branchPos = new StagePosition(playerPos.m_route, i, 0);
            if (IsValidSquare(branchPos))
            {
                nextPositions.Add(branchPos);
            }
        }

        // 次のroute
        int nextRouteIndex = playerPos.m_route + 1;
        if (nextRouteIndex < stageData.stageRoute.routeList.Count)
        {
            var nextRoute = stageData.stageRoute.routeList[nextRouteIndex];
            for (int i = 0; i < nextRoute.roadList.Count; i++)
            {
                StagePosition nextRoutePos = new StagePosition(nextRouteIndex, i, 0);
                if (IsValidSquare(nextRoutePos))
                {
                    nextPositions.Add(nextRoutePos);
                }
            }
        }

        // ループ対応
        if (nextPositions.Count == 0)
        {
            // ループの最初へ戻る
            StagePosition loopStart = new StagePosition(0, 0, 0);
            if (IsValidSquare(loopStart))
            {
                nextPositions.Add(loopStart);
            }
        }

        return nextPositions;
    }

    private bool IsValidSquare(StagePosition pos)
    {
        if (pos.m_route >= stageData.stageRoute.routeList.Count) return false;

        var roadList = stageData.stageRoute.routeList[pos.m_route].roadList;
        if (pos.m_road >= roadList.Count) return false;

        var squareList = roadList[pos.m_road].squareList;
        if (pos.m_square >= squareList.Count) return false;

        return squareList[pos.m_square] != null;
    }

    /// <summary>
    /// スターを置く位置を決定しマスにスターを付与
    /// </summary>
    /// <param name="characterPositionList"></param>
    public void DecideStarSquare(List<StagePosition> characterPositionList)
    {
        // プレイヤーの数分道を保持
        List<List<StagePosition>> squareListList = new List<List<StagePosition>>();

        for (int i = 0; i < characterPositionList.Count; i++)
        {
            // プレイヤーからプレイヤーのマスを取得
            List<StagePosition> squareList = GetPlayerToPlayerSquare(characterPositionList[i]);
            squareListList.Add(squareList);
        }

        // 昇順にソート
        squareListList.Sort();

        // 一番要素数の多いリストの最後の数のマスにスターを付与
        int listCount = squareListList[0].Count;
        Square starSquare = GetSquare(squareListList[0][listCount - 1]);

        // スターマスに設定
        starSquare.SetIsStarSquare(true);
    }

    /// <summary>
    /// プレイヤーとプレイヤーとの間のマスを取得
    /// </summary>
    /// <param name="startPosition"></param>
    /// <returns></returns>
    private List<StagePosition> GetPlayerToPlayerSquare(StagePosition startPosition)
    {
        StagePosition position = startPosition;

        List<StagePosition> playerToPlayerList = new List<StagePosition>();

        // ターゲットマスまで進んだら次
        while (true)
        {
            // 次のマス目を見る
            Square square = GetSquare(position);
            List<StagePosition> nextPositionList = square.GetNextPosition();
            // 分岐先があるならそのルートはスキップ
            if (nextPositionList.Count > 0)
            {
                position.m_route++;
                continue;
            }

            // 道を進め、カウントアップする
            position = nextPositionList[0];
            playerToPlayerList.Add(position);

            // キャラクターとぶつかったら
            if (GetSquare(position).standingPlayerList == null) break;
        }

        // ルートをリストで渡す
        return playerToPlayerList;
    }

    /// <summary>
    /// StagePosition    Vector3 ɕϊ     
    /// </summary>
    /// <param name="squarePosition"></param>
    /// <returns></returns>
    public Vector3 GetPosition(StagePosition squarePosition)
    {
        GameObject squareObject = stageData.stageRoute.routeList[squarePosition.m_route].roadList[squarePosition.m_road].squareList[squarePosition.m_square];
        return squareObject.transform.position;
    }

    /// <summary>
    /// カメラのアンカーを取得
    /// </summary>
    /// <returns></returns>
    public Transform GetCameraAnchor()
    {
        return stageData.cameraAnchor;
    }
}
