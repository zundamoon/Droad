using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuStatusEvent : MenuStatusItem
{
    [SerializeField]
    private TextMeshProUGUI _eventText = null;

    public override async UniTask Initialize()
    {
        await base.Initialize();
    }

    public void SetEvent(string setEvent)
    {
        _eventText.text = setEvent;
    }
}
