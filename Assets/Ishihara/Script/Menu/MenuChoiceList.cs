using System.Collections.Generic;
using UnityEngine;
using static CommonModule;

public class MenuChoiceList : MonoBehaviour
{
    /// <summary>
    /// カード選択肢のプレハブ
    /// </summary>
    [SerializeField]
    private MenuChoiceCard _choiceCardOriginal = null;

    /// <summary>
    /// 表示中のカードを並べる親オブジェクト
    /// </summary>
    [SerializeField]
    private Transform _contentRoot = null;

    /// <summary>
    /// 非表示状態のカードを格納する親オブジェクト
    /// </summary>
    [SerializeField]
    private Transform _unuseRoot = null;

    private const int _maxCardItem = 3;

    private List<MenuChoiceCard> _useCardList = null;
    private List<MenuChoiceCard> _unuseCardList = null;

    private System.Action<int> _OnSelect;

    public void SetOnselect(System.Action<int> onSelect)
    {
        _OnSelect = onSelect;
    }

    /// <summary>
    /// 初期化処理（カードオブジェクトを事前に生成して未使用リストに格納）
    /// </summary>
    public void Initialized()
    {
        _useCardList = new List<MenuChoiceCard>(_maxCardItem);
        _unuseCardList = new List<MenuChoiceCard>(_maxCardItem);

        for (int i = 0; i < _maxCardItem; i++)
        {
            // プレハブからカードを生成し未使用リストへ
            var item = Instantiate(_choiceCardOriginal, _unuseRoot);
            _unuseCardList.Add(item);
        }
    }

    /// <summary>
    /// カードIDに対応するカードを追加
    /// </summary>
    public void SetChoiceList(int choiceCardID)
    {
        // 未使用から取得または新規生成
        MenuChoiceCard addItem = AddListItem();

        // カード内容設定
        addItem.SetCard(choiceCardID);

        // 押下時のアクション設定
        addItem.SetButtonAction(() =>
        {
            _OnSelect(choiceCardID);
        });
    }

    /// <summary>
    /// カードIDに対応するカードを追加
    /// </summary>
    public void SetChoiceList(int choiceCardID, string buttonText)
    {
        // 未使用から取得または新規生成
        MenuChoiceCard addItem = AddListItem();

        // カード内容設定
        addItem.SetCard(choiceCardID);

        addItem.SetButtonText(buttonText);

        // 押下時のアクション設定
        addItem.SetButtonAction(() =>
        {
            _OnSelect(choiceCardID);
        });
    }

    /// <summary>
    /// カード項目を未使用リストから取得、または生成
    /// </summary>
    protected MenuChoiceCard AddListItem()
    {
        MenuChoiceCard addItem;
        if (IsEmpty(_unuseCardList))
        {
            // 未使用がないなら新しく生成
            addItem = Instantiate(_choiceCardOriginal, _contentRoot);
        }
        else
        {
            // 未使用から再利用
            addItem = _unuseCardList[0];
            _unuseCardList.RemoveAt(0);
            addItem.transform.SetParent(_contentRoot, false);
        }

        _useCardList.Add(addItem);
        return addItem;
    }

    /// <summary>
    /// 使用中のカードをすべて未使用に戻す
    /// </summary>
    public void RemoveList()
    {
        foreach (var item in _useCardList)
        {
            item.transform.SetParent(_unuseRoot, false);
            item.InitButtonText();
            _unuseCardList.Add(item);
        }
        _useCardList.Clear();
    }
}
