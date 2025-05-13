using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public static Action<int> OnUseCard = null;

    [SerializeField]
    private TextMeshProUGUI _advanceText = null;
    [SerializeField]
    private TextMeshProUGUI _coinText = null;
    [SerializeField]
    private TextMeshProUGUI _eventText = null;
    private int _ID = -1;
    private Transform _handArea;

    public void Start()
    {
        _handArea = transform.parent;
    }

    // ドラッグ
    public void OnDrag()
    {
        if (!UIManager.Instance.IsHandAccept) return;
        
        transform.position = Input.mousePosition;
    }

    // ドラッグ開始されたとき
    public void OnStartDrop()
    {
        if (!UIManager.Instance.IsHandAccept) return;

        Transform field = UIManager.Instance.GetHandCanvas().transform;
        // ドラッグしたオブジェクトを親から外す
        transform.SetParent(field);
        // 大きくする
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    // ドラッグ解除されたとき
    public void OnEndDrop()
    {
        if (!UIManager.Instance.IsHandAccept) return;

        // 元のサイズに戻す
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // 使用エリアなら
        if (!UIManager.Instance.CheckPlayArea(Input.mousePosition))
        {
            // 使用エリア外なら元の位置に戻す
            transform.SetParent(_handArea);
            return;
        }
        
        // カード使用
        Debug.Log(CardManager.GetCard(_ID).advance + "進む");
        Debug.Log(CardManager.GetCard(_ID).addCoin + "コイン獲得");
        //OnUseCard(_ID);
        // 入力受付終了
        UIManager.Instance.EndHandAccept();
  
        Destroy(gameObject);
    }

    /// <summary>
    /// カード情報を設定
    /// </summary>
    /// <param name="setID"></param>
    public void SetCard(int setID)
    {
        _ID = setID;
        // カード情報取得
        CardData card = CardManager.GetCard(_ID);
        if (card == null) return;

        _advanceText.text = card.advance.ToString();
        _coinText.text = card.addCoin.ToString();
        Entity_EventData.Param param = EventMasterUtility.GetEventMaster(card.eventID);
        if (param == null)
        {
            _eventText.text = "";
            return;
        }
        int eventTextID = param.textID;
        _eventText.text = eventTextID.ToText();
    }
}
