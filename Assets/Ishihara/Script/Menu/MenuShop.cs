using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using static CommonModule;

public class MenuShop : BaseMenu
{
    /// <summary>
    /// カード選択用メニュー
    /// </summary>
    [SerializeField]
    private MenuChoice _menuChoice = null;

    /// <summary>
    /// 「購入」ボタン
    /// </summary>
    [SerializeField]
    private Button _buyButton = null;

    /// <summary>
    /// 「除外」ボタン
    /// </summary>
    [SerializeField]
    private Button _removalButton = null;

    private List<int> _buyCardIDList = null;
    private List<int> _removalCardIDList = null;

    private UniTaskCompletionSource _isShopActive = null;

    /// <summary>
    /// 非同期初期化
    /// </summary>
    public override async UniTask Initialize()
    {
        await base.Initialize();
        await _menuChoice.Initialize();
        _buyCardIDList = new List<int>();
        _removalCardIDList = new List<int>();
    }

    public async UniTask SetSelectCallback(System.Action<int, bool> onSelect)
    {
        _menuChoice.SetSelectCallback((cardID) =>
        {
            bool isRemove = _buyButton.interactable;
            onSelect(cardID, isRemove);
            if (isRemove)
                RemovalActive();
            else
                BuyActive();
        });

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 購入候補カードのIDを設定
    /// </summary>
    public void SetBuyCardID(List<int> setBuyCardID)
    {
        _buyCardIDList = new List<int>(setBuyCardID);
    }

    /// <summary>
    /// 除外候補カードのIDを設定
    /// </summary>
    public void SetRemovalCardID(List<int> setRemovalCardID)
    {
        _removalCardIDList = new List<int>(setRemovalCardID);
    }

    /// <summary>
    /// メニューを開く
    /// </summary>
    public override async UniTask Open()
    {
        _isShopActive = new UniTaskCompletionSource();
        await base.Open();
        // デフォルトで購入モードを表示
        BuyActive(); 

        // 購入されるまで待つ
        await _isShopActive.Task;
    }

    /// <summary>
    /// メニューを閉じる
    /// </summary>
    public async void Close()
    {
        await base.Close(); 
        _isShopActive.TrySetResult();
        await UniTask.Yield();
    }

    /// <summary>
    /// 購入モードに切り替える
    /// </summary>
    public void BuyActive()
    {
        _buyButton.interactable = false;
        _removalButton.interactable = true;

        _menuChoice.RemoveAllItem();
        List<string> ButtonText = new List<string>(_buyCardIDList.Count);
        for (int i = 0; i < _buyCardIDList.Count; i++)
        {
            int CardID = _buyCardIDList[i];
            var Card = CardManager.instance.GetCard(CardID);
            ButtonText.Add(Card.price.ToString());
        }
        _menuChoice.SetChoiceButtonText(ButtonText);
        _menuChoice.SetChoiceCardID(_buyCardIDList);
        // 非同期処理の開始
        _menuChoice.Open().Forget(); 
    }

    /// <summary>
    /// 除外モードに切り替える
    /// </summary>
    public void RemovalActive()
    {
        _buyButton.interactable = true;
        _removalButton.interactable = false;

        _menuChoice.RemoveAllItem();
        List<string> ButtonText = new List<string>(_removalCardIDList.Count);
        for (int i = 0; i < _removalCardIDList.Count; i++)
        {
            int CardID = _removalCardIDList[i];
            var Card = CardManager.instance.GetCard(CardID);
            ButtonText.Add(Card.price.ToString());
        }
        _menuChoice.SetChoiceButtonText(ButtonText);
        _menuChoice.SetChoiceCardID(_removalCardIDList);
        _menuChoice.Open().Forget();
    }

    /// <summary>
    /// ショップのアイテム削除
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="isRemove"></param>
    /// <returns></returns>
    public async UniTask RemoveShopItem(int cardID, bool isRemove)
    {
        if (isRemove)
        {
            _removalCardIDList.Remove(cardID);
            RemovalActive();
        }
        else
        {
            _buyCardIDList.Remove(cardID);
            BuyActive();
        }

        await UniTask.CompletedTask;
    }

    public async UniTask RemoveAllShopItem()
    {
        _menuChoice.RemoveAllItem();
        _buyCardIDList.Clear();
        _removalCardIDList.Clear();
    }
}
