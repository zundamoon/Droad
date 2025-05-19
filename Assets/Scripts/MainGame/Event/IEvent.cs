using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public interface IEvent
{
    UniTask ExecuteEvent(EventContext context, int param);
}
