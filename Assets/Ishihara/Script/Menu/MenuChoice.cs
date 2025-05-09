using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CommonModule;

class MenuListItem : MonoBehaviour
{
    public void Deselect()
    {
        // 実装は省略
    }
}

public class MenuList : BaseMenu
{
    /// <summary>
    /// リスト項目のオリジナル
    /// </summary>
    [SerializeField]
    private MenuListItem _itemOrigin = null;
    /// <summary>
    /// 項目を並べるルートオブジェクト
    /// </summary>
    [SerializeField]
    private Transform _contentRoot = null;
    /// <summary>
    /// 未使用状態の項目のルートオブジェクト
    /// </summary>
    [SerializeField]
    private Transform _unuseRoot = null;

    private int _currentIndex = -1;

    private List<MenuListItem> _useList = null;
    private List<MenuListItem> _unuseList = null;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        _useList = new List<MenuListItem>();
        _unuseList = new List<MenuListItem>();
    }

    /// <summary>
    /// リスト項目の生成
    /// </summary>
    /// <returns></returns>
    protected MenuListItem AddListItem()
    {
        MenuListItem addItem;
        if (IsEmpty(_unuseList))
        {
            // 未使用リストが空なので生成
            addItem = Instantiate(_itemOrigin, _contentRoot);
        }
        else
        {
            // 未使用リストから取得
            addItem = _unuseList[0];
            _unuseList.RemoveAt(0);
            addItem.transform.SetParent(_contentRoot);
        }
        addItem.Deselect();
        _useList.Add(addItem);
        return addItem;
    }

    /// <summary>
    /// インデクス指定のリスト項目削除
    /// </summary>
    /// <param name="itemIndex"></param>
    protected void RemoveListItem(int itemIndex)
    {
        if (!IsEnableIndex(_useList, itemIndex)) return;
        // 使用リストから取り除く
        MenuListItem removeItem = _useList[itemIndex];
        _useList.RemoveAt(itemIndex);
        // 未使用リストへ追加
        _unuseList.Add(removeItem);
        removeItem.transform.SetParent(_unuseRoot);
        removeItem.Deselect();
    }

    /// <summary>
    /// 全てのリスト項目削除
    /// </summary>
    protected void RemoveAllItem()
    {
        while (!IsEmpty(_useList)) RemoveListItem(0);

    }
}