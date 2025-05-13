using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static CommonModule;

public class MenuChoiceList : MonoBehaviour
{
    // 保持するカード選択肢のオリジナル
    [SerializeField]
    private MenuChoiceCard _choiceCardOriginal = null;

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

    private const int _maxCardItem = 3;

    private List<MenuChoiceCard> _useCardList = null;
    private List<MenuChoiceCard> _unuseCardList = null;

    public void Initialized()
    {
        _unuseCardList = new List<MenuChoiceCard>(_maxCardItem);
        _useCardList = new List<MenuChoiceCard>(_maxCardItem);
        for (int i = 0; i < _maxCardItem; i++)
        {
            // 未使用リストに追加
            var item = Instantiate(_choiceCardOriginal, _unuseRoot);
            _unuseCardList.Add(item);
        }
    }

    public void SetChoiceList(int choicecardID)
    {
        // 未使用リストから取得
        MenuChoiceCard addItem = AddListItem();
        addItem.SetCard(choicecardID);
        _useCardList.Add(addItem);
        // ボタンアクション
        addItem.SetButtonAction(() =>
        {
            // 選択肢を選択した時の処理
            Debug.Log($"選択肢{choicecardID}が選択されました");
        });
    }

    /// <summary>
    /// リスト項目の生成
    /// </summary>
    /// <returns></returns>
    protected MenuChoiceCard AddListItem()
    {
        MenuChoiceCard addItem;
        if (IsEmpty(_unuseCardList))
        {
            // 未使用リストが空なので生成
            addItem = Instantiate(_choiceCardOriginal, _contentRoot);
        }
        else
        {
            // 未使用リストから取得
            addItem = _unuseCardList[0];
            _unuseCardList.RemoveAt(0);
            addItem.transform.SetParent(_contentRoot);
        }
        _useCardList.Add(addItem);
        return addItem;
    }

    public void RemoveList()
    {
        for(int i = 0; i < _useCardList.Count; i++)
        {
            // 未使用エリアに移動
            var item = _useCardList.FirstOrDefault();
            if (item == null) continue;
            item.transform.SetParent(_unuseRoot);
            _unuseCardList.Add(item);
            _useCardList.Remove(item);
        }
    }
}
