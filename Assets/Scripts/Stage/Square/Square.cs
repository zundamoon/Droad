using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameEnum;

public abstract class Square : MonoBehaviour
{
    [SerializeField] public SquareType squareType = SquareType.INVALID;
    [SerializeField] public bool isStopSquare = false;
    [SerializeField] public bool isPointSquare = false;
    [SerializeField] public new Renderer renderer = null;
    [SerializeField] protected int eventID = -1;

    protected List<int> standingPlayerList = null;

    public bool GetPoint() { return isPointSquare; }
    public abstract int GetEventID();
    public void SetEventID(int eventID) { this.eventID = eventID; }
}
