using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event011_UnluckyEvent : BaseEvent
{
    /// <summary>
    /// �C�x���g��ID���X�g
    /// </summary>
    private int[] _eventIDList = { };

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        // �C�x���g�̒��I
        int eventID = DicideEvent();
        await EventManager.ExecuteEvent(eventID, context);
    }

    /// <summary>
    /// �C�x���gID�����߂�
    /// </summary>
    /// <returns></returns>
    public int DicideEvent()
    {
        int index = Random.Range(0, _eventIDList.Length);
        return _eventIDList[index];
    }
}
