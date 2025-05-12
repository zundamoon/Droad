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
    public bool CheckStopSquare(StagePosition squarePos) { return GetSquare(squarePos).isStopSquare; }

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

    public StagePosition CheckNextPosition(StagePosition playerPos)
    {
        StagePosition nextPos = playerPos;
        nextPos.m_square += 1;

        var routeList = stageData.stageRoute.routeList;

        // ���̃}�X�Ɉړ��ł��邩�`�F�b�N
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

        // ���̓��ɐi�ޏ���
        nextPos.m_route += 1;
        nextPos.m_road = 0;
        nextPos.m_square = 0;

        // ���̓��Ń}�X�ړ��ł��邩���`�F�b�N
        if (nextPos.m_route < routeList.Count)
        {
            // �����X�g
            var nextRoadList = routeList[nextPos.m_route].roadList;

            if (nextRoadList.Count > 0)
            {
                // �}�X��
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
