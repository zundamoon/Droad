using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _advanceText = null;
    [SerializeField]
    private TextMeshProUGUI _coinText = null;
    [SerializeField]
    private TextMeshProUGUI _eventText = null;

    /// <summary>
    /// テキストの設定
    /// </summary>
    /// <param name="advanceText"></param>
    /// <param name="coinText"></param>
    /// <param name="eventText"></param>
    public void SetText(string advanceText, string coinText, string eventText)
    {
        _advanceText.text = advanceText;
        _coinText.text = coinText;
        _eventText.text = eventText;
    }
}
