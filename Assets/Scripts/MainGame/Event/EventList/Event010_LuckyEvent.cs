using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event010_LuckyEvent : BaseEvent
{
    /// <summary>
    /// �C�x���gID�ƒ��I����
    /// </summary>
    private static Dictionary<int, int> eventWeights = new Dictionary<int, int>
    {
        { 30, 10 },
        { 32, 20 },
        { 2, 20 },
        { 13, 20 },
        { 34, 20 },
    };

    private const int _LUCKEY_TEXT_ID = 110;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        await UIManager.instance.RunMessage(_LUCKEY_TEXT_ID.ToText());
        // �C�x���g�̒��I
        int eventID = DicideEventID();
        await EventManager.ExecuteEvent(eventID, context);
    }

    /// <summary>
    /// �C�x���gID�����߂�
    /// </summary>
    /// <returns></returns>
    private int DicideEventID()
    {
        // �d�݂��擾
        int totalRatio = 0;
        foreach (var weight in eventWeights.Values)
        {
            totalRatio += weight;
        }
        int randomValue = Random.Range(0, totalRatio);

        // �d�݂��烌�A���e�B��I�o
        int currentRatio = 0;
        foreach (var pair in eventWeights)
        {
            currentRatio += pair.Value;
            if (randomValue < currentRatio)
            {
                return pair.Key;
            }
        }
        return -1;
    }
}
