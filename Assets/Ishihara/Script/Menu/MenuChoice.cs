using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CommonModule;

public class MenuChoice : BaseMenu
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
    /// 未使用状態の項目のルートオブジェクト
    /// </summary>
    [SerializeField]
    private Transform _unuseRoot = null;

    private List<MenuChoiceList> _useList = null;
    private List<MenuChoiceList> _unuseList = null;

    private List<int> _choiceCardIDList = null;

    private List<string> _choiceButtonTex = null;

    private System.Action<int> _OnSelect = null;

    public void SetSelectCallback(System.Action<int> onSelect)
    {
        _OnSelect = onSelect;
    }

    public void SetChoiceButtonText(List<string> strings)
    {
        _choiceButtonTex = new List<string>(strings);
    }

    public override async UniTask Initialize()
    {
        await base.Initialize();
        _useList = new List<MenuChoiceList>();
        _unuseList = new List<MenuChoiceList>();
        _choiceCardIDList = new List<int>();
    }

    public void SetChoiceCardID(List<int> setChoiceCardID)
    { 
        _choiceCardIDList = new List<int>(setChoiceCardID); ;
    }

    public async override UniTask Open()
    {
        if (_OnSelect == null) return;

        await base.Open();
        // 並べる
        // 選択肢が何行必要か
        for(int i = 0; i < _choiceCardIDList.Count; i+= 3)
        {
            // リスト項目を生成
            MenuChoiceList item = AddListItem();
            for (int j = 0; j < 3; j++)
            {
                // 3つ目の選択肢が無い場合はスキップ
                if (i + j >= _choiceCardIDList.Count) break;
                // リスト項目にカード情報をセット
                if(IsEnableIndex(_choiceButtonTex, i + j))
                {
                    item.SetChoiceList(_choiceCardIDList[i + j] ,_choiceButtonTex[i + j]);
                }
                else
                {
                    item.SetChoiceList(_choiceCardIDList[i + j]);
                }
            }
        }
    }

    /// <summary>
    /// リスト項目の生成
    /// </summary>
    /// <returns></returns>
    protected MenuChoiceList AddListItem()
    {
        MenuChoiceList addItem;
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
        addItem.Initialized();
        addItem.SetOnselect(_OnSelect);
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
        MenuChoiceList removeItem = _useList[itemIndex];
        removeItem.RemoveList();
        _useList.RemoveAt(itemIndex);
        // 未使用リストへ追加
        _unuseList.Add(removeItem);
        removeItem.transform.SetParent(_unuseRoot);
    }

    /// <summary>
    /// 全てのリスト項目削除
    /// </summary>
    public void RemoveAllItem()
    {
        while (!IsEmpty(_useList)) RemoveListItem(0);
    }
}