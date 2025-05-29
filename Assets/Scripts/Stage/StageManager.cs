using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static CommonModule;
using static GameConst;
using static GameEnum;
using Unity.VisualScripting;

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
        InitAllSquare();
    }

    /// <summary>
    ///  ステージを生成
    /// </summary>
    public void GenerateStage()
    {
        Instantiate(stagePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
    }

    /// <summary>
    /// Squareの情報を初期化
    /// </summary>
    public void InitAllSquare()
    {
        for (int i = 0; i < stageData.stageRoute.routeList.Count; i++)
        {
            var roadList = stageData.stageRoute.routeList[i];

            for (int j = 0; j < roadList.roadList.Count; j++)
            {
                var squareList = roadList.roadList[j];

                for (int k = 0; k < squareList.squareList.Count; k++)
                {
                    var squareObject = squareList.squareList[k];
                    if (squareObject == null) continue;

                    var square = squareObject.GetComponent<Square>();
                    if (square == null) continue;

                    StagePosition pos = new StagePosition(i, j, k);
                    square.SetPosition(pos);

                    // 初期化処理
                    square.Init();

                    List<StagePosition> nextPositions = CheckNextPosition(pos);
                    square.SetNextPosition(nextPositions);
                }
            }
        }
    }

    /// <summary>
    /// Squareの種類を変更する
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="type"></param>
    public void ChangeSquareType(StagePosition pos, SquareType type)
    {
        Square square = GetSquare(pos);
        BaseSquareData newData = CreateSquareData(type);

        if (newData != null)
        {
            square.ChangeSquareType(newData);
        }
    }

    /// <summary>
    /// SquareDataのファクトリ
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Squareを取得
    /// </summary>
    /// <param name="squarePos"></param>
    /// <returns></returns>
    public Square GetSquare(StagePosition squarePos)
    {
        GameObject squareObject = stageData.stageRoute.routeList[squarePos.m_route].roadList[squarePos.m_road].squareList[squarePos.m_square];
        Square square = squareObject.GetComponent<Square>();

        return square;
    }

    /// <summary>
    /// Squareを取得
    /// </summary>
    /// <param name="squareObject"></param>
    /// <returns></returns>
    public Square GetSquare(GameObject squareObject)
    {
        Square square = squareObject.GetComponent<Square>();

        return square;
    }

    public int GetSquareEvent(StagePosition squarePos) { return GetSquare(squarePos).GetEventID(); }

    public bool CheckStopSquare(StagePosition squarePos) { return GetSquare(squarePos).GetIsStopSquare(); }

    public StagePosition GetNextPosition(StagePosition playerPos)
    {
        return GetSquare(playerPos).GetSquareData().nextPositionList[0];
    }

    /// <summary>
    /// 次に移動できるマスを取得
    /// </summary>
    /// <param name="playerPos"></param>
    /// <returns></returns>
    public List<StagePosition> CheckNextPosition(StagePosition playerPos)
    {
        List<StagePosition> nextPositions = new List<StagePosition>();

        // 同じroad内の次マスが存在するならそれだけ返す
        StagePosition nextInSameRoad = new StagePosition(playerPos.m_route, playerPos.m_road, playerPos.m_square + 1);
        if (IsValidSquare(nextInSameRoad))
        {
            nextPositions.Add(nextInSameRoad);
            return nextPositions;
        }

        // この道の終端だったら、次のrouteに進む
        int nextRouteIndex = playerPos.m_route + 1;
        if (nextRouteIndex < stageData.stageRoute.routeList.Count)
        {
            var nextRoute = stageData.stageRoute.routeList[nextRouteIndex];

            // 次のrouteのすべてのroadの先頭マスを取得
            for (int roadIndex = 0; roadIndex < nextRoute.roadList.Count; roadIndex++)
            {
                StagePosition nextRoutePos = new StagePosition(nextRouteIndex, roadIndex, 0);
                if (IsValidSquare(nextRoutePos))
                {
                    nextPositions.Add(nextRoutePos);
                }
            }
        }

        // 全てのルートが終わっていた場合はループ
        if (nextPositions.Count == 0)
        {
            StagePosition loopPos = new StagePosition(1, 0, 0); // 最初のマス
            if (IsValidSquare(loopPos))
            {
                nextPositions.Add(loopPos);
            }
        }

        return nextPositions;
    }

    /// <summary>
    /// リストを確認
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
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
    public void DecideStarSquare()
    {
        List<StagePosition> characterPositionList = new List<StagePosition>();

        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            StagePosition pos = CharacterManager.instance.GetCharacter(i).position;
            characterPositionList.Add(pos);
        }

        // 各プレイヤーの位置からどれだけ進めるかを調べる
        List<StagePosition> squareList = new List<StagePosition>();

        foreach (StagePosition startPos in characterPositionList)
        {
            List<StagePosition> path = GetPlayerToPlayerSquare(startPos);
            // 入ってるマス目が多いリストに更新
            if (path.Count > squareList.Count) squareList = path;
        }

        // 最後のマスにスターを置く
        if (squareList.Count > 0)
        {
            Square square = null;
            while (true)
            {
                // マスの抽選
                int value = UnityEngine.Random.Range(1, 5);
                StagePosition lastSquarePos = squareList[squareList.Count - value];
                square = GetSquare(lastSquarePos);

                // 分岐マスへの設置を防ぐ
                if (square.GetSquareData() is not BranchSquare) break;
            }

            square.ChangeStarSquare();
        }
    }

    /// <summary>
    /// プレイヤーから次に出会うプレイヤーまでのマス一覧を取得
    /// </summary>
    private List<StagePosition> GetPlayerToPlayerSquare(StagePosition startPosition)
    {
        List<StagePosition> path = new List<StagePosition>();
        StagePosition currentPos = startPosition;

        // 一度通ったマスを管理
        List<StagePosition> visited = new List<StagePosition>();

        while (true)
        {
            Square currentSquare = GetSquare(currentPos);
            List<StagePosition> nextList = currentSquare.GetNextPosition();

            if (nextList == null || nextList.Count == 0) break;

            currentPos = nextList[0];

            // 重複した値がないか確認
            if (visited.Contains(currentPos)) break;

            Square nextSquare = GetSquare(currentPos);

            // 他のプレイヤーがいるなら終了
            if (nextSquare.standingPlayerList != null && nextSquare.standingPlayerList.Count > 0)
            {
                break;
            }

            // マスがあるならリストに追加
            visited.Add(currentPos);
            path.Add(currentPos);
        }

        return path;
    }

    /// <summary>
    /// StagePositionからワールド座標に変換   
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
