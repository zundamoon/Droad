using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameEnum;

public class Square : MonoBehaviour
{
    public bool isStarSquare;
    [SerializeField] public List<int> standingPlayerList = null;
    [SerializeField] public List<GameObject> standAnchorList = null;
    [SerializeField] private new Renderer renderer;
    [SerializeReference, SubclassSelector] private BaseSquareData squareData;

    public void Init()
    {
        isStarSquare = false;
        ChangeLooks();
    }

    public void ChangeStarSquare()
    {
        if (isStarSquare) isStarSquare = false;
        else isStarSquare = true;
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

    public int GetEventID() { return squareData.eventID; }
    public bool GetIsStarSquare() { return isStarSquare; }
    public void SetIsStarSquare(bool state) { isStarSquare = state; }
}
