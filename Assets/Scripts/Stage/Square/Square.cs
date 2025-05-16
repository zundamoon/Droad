using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnum;
using static CommonModule;

public class Square : MonoBehaviour
{
    public bool isStarSquare;
    [SerializeField] public List<int> standingPlayerList = null;
    [SerializeField] public List<GameObject> standAnchorList = null;
    [SerializeField] private new Renderer renderer;
    [SerializeReference, SubclassSelector] private BaseSquareData squareData;
    // ステージ上のマスの位置
    public StagePosition squarePosition { get; protected set; }
    // 次に移動できるマスの候補
    public List<StagePosition> nextPositionList { get; protected set; }

    public void Init(List<StagePosition> positionList)
    {
        isStarSquare = false;
        nextPositionList = positionList;
        ChangeLooks();
    }

    public void ChangeStarSquare()
    {
        if (isStarSquare) isStarSquare = false;
        else
        {
            isStarSquare = true;
        }
    }

    public void ChangeSquareType(BaseSquareData baseSquareData)
    {
        if (baseSquareData == null) return;

        squareData = baseSquareData;
        ChangeLooks();
    }

    private void ChangeLooks()
    {
        renderer.material.color = squareData.squareColor;
    }

    public BaseSquareData GetSquareData() { return squareData; }
    public StagePosition GetSquarePosition() { return squarePosition; }
    public List<StagePosition> GetNextPosition() { return nextPositionList; }
    public int GetEventID() { return squareData.eventID; }
    public bool GetIsStarSquare() { return isStarSquare; }
    public void SetIsStarSquare(bool state) { isStarSquare = state; }
}
