using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MenuHand : BaseMenu
{
    /// <summary>
    /// リスト項目のオリジナル
    /// </summary>
    [SerializeField]
    private GameObject _itemOrigin = null;
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
            item.gameObject.SetActive(true);
            _unuseList.Add(item.GetComponent<CardObject>());
        }
    }

    public async override UniTask Open()
    {
        await base.Open();
        // 並べる
        // 手札枚数取得
        int handCount = _possessCard.handCardIDList.Count;
        // 並べる
        for (int i = 0; i < handCount; i++)
        {
            // 使用エリアに移動
            var item = _unuseList.FirstOrDefault();
            if (item == null) continue;
            item.gameObject.SetActive(true);
            // カード情報更新
            //item.SetCard(_possessCard.handCardIDList[i]);
            item.transform.SetParent(_contentRoot);
            _useList.Add(item);
            _unuseList.Remove(item);
        }
    }

    public async void Update()
    {
        await UniTask.DelayFrame(1);
    }

    public RectTransform GetPlayArea()
    {
        return _playArea;
    }
}
