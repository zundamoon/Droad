using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class BaseEvent : IEvent
{
    public abstract UniTask ExecuteEvent(EventContext context, int param);
}
