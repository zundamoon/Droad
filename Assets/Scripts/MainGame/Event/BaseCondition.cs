using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class BaseCondition : ICondition
{
    /// <summary>
    /// ������B�����Ă��邩
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public abstract UniTask<bool> IsCompleteCondition(EventContext context, int param);
}
