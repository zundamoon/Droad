using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameEnum;

public class Square : MonoBehaviour
{
    [SerializeField] public SquareType squareType = SquareType.INVALID;
    [SerializeField] public bool isStopSquare = false;
    [SerializeField] public bool isStarSquare = false;
    [SerializeField] private int eventID = -1;
    [SerializeField] private new Renderer renderer = null;

    private void Start()
    {
        ChangeSquareType(squareType);
    }

    public void ChangeSquareType(SquareType type)
    {
        squareType = type;
        switch (type)
        {
            case SquareType.INVALID:
                renderer.material.color = Color.gray;
                break;
            case SquareType.BLUE:
                renderer.material.color = Color.blue;
                break;
            case SquareType.RED:
                renderer.material.color = Color.red;
                break;
            case SquareType.HAPPENING:
                renderer.material.color = Color.green;
                break;
            case SquareType.SHOP:
                renderer.material.color = Color.yellow;
                break;
        }

        return;
    }

    // public void GrantD

    public int GetEventID() { return eventID; }
}
