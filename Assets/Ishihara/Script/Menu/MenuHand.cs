using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

using static CommonModule;

public class MenuHand : BaseMenu
{
    /// <summary>
    /// リスト項目のオリジナル
    /// </summary>
    [SerializeField]
    private CardObject _itemOrigin = null;
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

    private List<CardObject> _useList = null;
    private List<CardObject> _unuseList = null;

    private PossessCard _possessCard = null;
    private readonly int _MAX_HAND_CARD = 4;

    public void SetTurnPlayerCard(PossessCard setPossessCard)
    {
        _possessCard = setPossessCard;
    }

    public async override UniTask Initialize()
    {
        await base.Initialize();
        _useList = new List<CardObject>();
        _unuseList = new List<CardObject>();
        for (int i = 0; i < _MAX_HAND_CARD; i++)
        {
            var item = Instantiate(_itemOrigin, _unuseRoot);
            _unuseList.Add(item);
        }
    }

    public async override UniTask Open()
    {
        RemoveAllItem();

        await base.Open();
        // 並べる
        // 手札枚数取得
        int handCount = _possessCard.handCardIDList.Count;
        // 並べる
        for (int i = 0; i < handCount; i++)
        {
            // 使用エリアに移動
            var item = AddListItem();
            // カード情報更新
            item.SetCard(_possessCard.handCardIDList[i]);
            item.SetHandIndex(i);
        }
    }

    protected CardObject AddListItem()
    {
        CardObject addItem;
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
        addItem.gameObject.SetActive(true);
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
        CardObject removeItem = _useList[itemIndex];
        _useList.RemoveAt(itemIndex);
        // 未使用リストへ追加
        _unuseList.Add(removeItem);
        removeItem.transform.SetParent(_unuseRoot);
    }

    /// <summary>
    /// 全てのリスト項目削除
    /// </summary>
    protected void RemoveAllItem()
    {
        while (!IsEmpty(_useList)) RemoveListItem(0);

    }

    public RectTransform GetPlayArea()
    {
        return _playArea;
    }
}
