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
    [SerializeField] protected EventID eventID = EventID.INVALID;

    protected List<int> standingPlayerList = null;

    public bool GetPoint() { return isPointSquare; }
    public abstract EventID GetEventID();
    public void SetEventID(EventID eventID) { this.eventID = eventID; }
}
