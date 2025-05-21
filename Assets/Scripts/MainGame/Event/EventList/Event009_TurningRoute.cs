using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event009_TurningRoute : BaseEvent
{
    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        Square square = context.square;
        if (character == null || square == null) return;

        BaseSquareData squareData = square.GetSquareData();
        BranchSquare branchSquare = squareData as BranchSquare;
        await branchSquare.SelectBranch(character);

    }
}
