using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;
using static CommonModule;

public class Square : MonoBehaviour
{
    public bool isStarSquare;
    public bool isStopSquare;
    [SerializeField] public List<int> standingPlayerList = null;
    [SerializeField] public List<GameObject> standAnchorList = null;
    [SerializeField] private new Renderer renderer;
    [SerializeReference, SubclassSelector] private BaseSquareData squareData;
    // ステージ上のマスの位置
    public StagePosition squarePosition { get; protected set; }
    // 次に移動できるマスの候補
    public List<StagePosition> nextPositionList { get; protected set; }

    public void Init()
    {
        isStarSquare = false;
        ChangeLooks();
    }
    /// <summary>
    /// スターマスかを切り替える
    /// </summary>
    public void ChangeStarSquare()
    {
        if (isStarSquare)
        {
            isStarSquare = false;
        }
        else
        {
            isStarSquare = true;
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

    public void SetPosition(StagePosition position) { squarePosition = position; }

    public void SetNextPosition(List<StagePosition> positions) { nextPositionList = positions; }
    public BaseSquareData GetSquareData() { return squareData; }
    public StagePosition GetSquarePosition() { return squarePosition; }
    public List<StagePosition> GetNextPosition() { return nextPositionList; }
    public int GetEventID() { return squareData.eventID; }
    public bool GetIsStarSquare() { return isStarSquare; }
    public void SetIsStarSquare(bool state) { isStarSquare = state; }

    public bool GetIsStopSquare() { return isStopSquare; }
    public void SetIsStopSquare(bool state) { isStopSquare = state; }
}
