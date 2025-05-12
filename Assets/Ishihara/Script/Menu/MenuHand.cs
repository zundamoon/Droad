using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHand : BaseMenu
{
    /// <summary>
    /// リスト項目のオリジナル
    /// </summary>
    [SerializeField]
    private MenuChoiceList _itemOrigin = null;
    /// <summary>
    /// 項目を並べるルートオブジェクト
    /// </summary>
    [SerializeField]
    private Transform _contentRoot = null;
    /// <summary>
    /// 使用エリアのルートオブジェクト
    /// </summary>
    [SerializeField]
    private RectTransform _playArea = null;
    /// <summary>
    /// 未使用状態の項目のルートオブジェクト
    /// </summary>
    [SerializeField]
    private Transform _unuseRoot = null;

    private List<MenuChoiceList> _useList = null;
    private List<MenuChoiceList> _unuseList = null;

    public RectTransform GetPlayArea()
    {
        return _playArea;
    }
}
