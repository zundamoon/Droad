using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class BaseEvent
{
    /// <summary>
    /// �C�x���g���s����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sourceCharacter"></param>
    /// <param name="param"></param>
    /// <param name="setParam"></param>
    public abstract UniTask PlayEvent(Character sourceCharacter, int param, Square square = null);
}
