using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEvent
{
    /// <summary>
    /// �C�x���g���s����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sourceCharacter"></param>
    /// <param name="param"></param>
    /// <param name="setParam"></param>
    public abstract void PlayEvent(Character sourceCharacter, int param, int addParam = -1);
}
