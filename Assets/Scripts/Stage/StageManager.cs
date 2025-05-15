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
                        square.Init();
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

    /// <summary>
    /// �n���ꂽ���W�̈�}�X��̍��W���m�F
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

    //    // �i�߂�}�X���Ȃ��Ȃ��Ă��Ȃ���������W��Ԃ�
    //    if (nextSquare != null) return nextPos;
    //    nextPos.m_route += 1;
    //    // ���̃��[�g�ɂ����������邩�擾
    //    int roadCount = stageData.stageRoute.routeList[nextPos.m_route].roadList.Count;
    //    nextPos.m_road = roadCount;
    //    nextPos.m_square = 0;

    //    return nextPos;
    //}

    /// <summary>
    /// �n���ꂽ���W�̈�}�X��̍��W���m�F
    /// </summary>
    /// <param name="playerPos"></param>
    /// <returns></returns>
    public StagePosition CheckNextPosition(StagePosition playerPos)
    {
        StagePosition nextPos = playerPos;
        nextPos.m_square++;

        if (IsValidSquare(nextPos)) return nextPos;

        // ���̃��[�g�Ɉړ�
        nextPos.m_route++;
        nextPos.m_road = 0;
        nextPos.m_square = 0;

        if (IsValidSquare(nextPos)) return nextPos;

        // 1�Ԗڂ̃��[�g�֖߂�
        return new StagePosition
        {
            m_route = 1,
            m_road = 0,
            m_square = 0
        };
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
    /// StagePosition����Vector3�ɕϊ�����
    /// </summary>
    /// <param name="squarePosition"></param>
    /// <returns></returns>
    public Vector3 GetPosition(StagePosition squarePosition)
    {
        GameObject squareObject = stageData.stageRoute.routeList[squarePosition.m_route].roadList[squarePosition.m_road].squareList[squarePosition.m_square];
        return squareObject.transform.position;
    }
}
