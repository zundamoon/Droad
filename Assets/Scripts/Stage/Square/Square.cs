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
    /// スターマスかを切り替える
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
    /// マスの種類を変更
    /// </summary>
    /// <param name="baseSquareData"></param>
    public void ChangeSquareType(BaseSquareData baseSquareData)
    {
        if (baseSquareData == null) return;

        squareData = baseSquareData;
        ChangeLooks();
    }
    /// <summary>
    /// リストにIDを追加
    /// </summary>
    /// <param name="playerID"></param>
    public void AddStandingList(int playerID)
    {
        standingPlayerList.Add(playerID);
    }
    /// <summary>
    /// リストからIDを削除
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
    /// 見た目の変更を適応
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
