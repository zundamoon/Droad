using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterAI : MonoBehaviour
{
    public abstract void Init();

    public abstract void Proc();

    public virtual int ChoiceCard(int handCardMax)
    {
        int index = -1;

        return index;
    }
}
