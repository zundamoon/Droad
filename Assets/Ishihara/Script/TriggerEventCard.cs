using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEventCard : MonoBehaviour
{
    private Transform _handArea;

    public void Start()
    {
        _handArea = transform.parent;
    }

    // ドラッグ
    public void OnDrag()
    {
        if (!UIManager.Instance.IsHandAccept) return;
        
        this.transform.position = Input.mousePosition;
    }

    // ドラッグ開始されたとき
    public void OnStartDrop()
    {
        if (!UIManager.Instance.IsHandAccept) return;

        Transform field = UIManager.Instance.GetHandCanvas().transform;
        // ドラッグしたオブジェクトを親から外す
        this.transform.SetParent(field);
    }

    // ドラッグ解除されたとき
    public void OnEndDrop()
    {
        if (!UIManager.Instance.IsHandAccept) return;

        // 使用エリアなら
        if (!UIManager.Instance.CheckPlayArea(Input.mousePosition))
        {
            // 使用エリア外なら元の位置に戻す
            this.transform.SetParent(_handArea);
            return;
        }
        
        // カード使用
        Debug.Log("カード使用");
        // 入力受付終了
        UIManager.Instance.EndHandAccept();
  
        Destroy(this.gameObject);
    }
}
