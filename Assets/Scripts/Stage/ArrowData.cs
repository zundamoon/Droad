using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class ArrowData : MonoBehaviour
{
    public StagePosition nextPosition;
    public int number;

    public void Init(StagePosition stagePosition, int arrayNumber)
    {
        nextPosition = stagePosition;
        number = arrayNumber;
    }
}
