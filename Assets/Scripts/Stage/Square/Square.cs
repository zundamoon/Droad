using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;
using static CommonModule;

public class Square : MonoBehaviour
{
    [SerializeField] public List<int> standingPlayerList = null;
    [SerializeField] public List<GameObject> standAnchorList = null;
    [SerializeField] private new Renderer renderer;
    [SerializeReference, SubclassSelector] private BaseSquareData squareData;

    public void Init()
    {
        GetSquareData().isStarSquare = false;
        ChangeLooks();
    }
    /// <summary>
    /// �X�^�[�}�X����؂�ւ���
    /// </summary>
    public void ChangeStarSquare()
    {
        if (GetIsStarSquare())
        {
            GetSquareData().isStarSquare = false;
        }
        else
        {
            GetSquareData().isStarSquare = true;
            squareData.ChangeColor(Color.magenta);
        }
        ChangeLooks();
    }
    /// <summary>
    /// �}�X�̎�ނ�ύX
    /// </summary>
    /// <param name="baseSquareData"></param>
    public void ChangeSquareType(BaseSquareData baseSquareData)
    {
        if (baseSquareData == null) return;

        squareData = baseSquareData;
        ChangeLooks();
    }
    /// <summary>
    /// ���X�g��ID��ǉ�
    /// </summary>
    /// <param name="playerID"></param>
    public void AddStandingList(int playerID)
    {
        standingPlayerList.Add(playerID);
    }
    /// <summary>
    /// ���X�g����ID���폜
    /// </summary>
    /// <param name="playerID"></param>
    public void DeleteStandingList(int playerID)
    {
        var index = standingPlayerList.IndexOf(playerID);
        if (index >= 0)
        {
            standingPlayerList.RemoveAt(index);
        }
    }
    /// <summary>
    /// �����ڂ̕ύX��K��
    /// </summary>
    private void ChangeLooks() { renderer.material.color = squareData.squareColor; }

    public BaseSquareData GetSquareData() { return squareData; }
    public void SetPosition(StagePosition position) { GetSquareData().squarePosition = position; }

    public void SetNextPosition(List<StagePosition> positions) { GetSquareData().nextPositionList = positions; }
    public StagePosition GetSquarePosition() { return GetSquareData().squarePosition; }
    public List<StagePosition> GetNextPosition() { return GetSquareData().nextPositionList; }
    public int GetEventID() { return squareData.eventID; }
    public bool GetIsStarSquare() { return GetSquareData().isStarSquare; }
    public void SetIsStarSquare(bool state) { GetSquareData().isStarSquare = state; }

    public bool GetIsStopSquare() { return GetSquareData().isStopSquare; }
}
