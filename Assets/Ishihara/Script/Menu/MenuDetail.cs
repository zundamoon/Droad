using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuDetail : BaseMenu
{
    [SerializeField]
    private MenuChoice _choice = null;

    [SerializeField]
    private Button _deckDetail = null;

    [SerializeField]
    private Button _discardAreaDetail = null;

    [SerializeField]
    private Button _closeButton = null;

    // 手札の入力が受付中かどうか
    private bool _IsHandAccept = false;

    private PossessCard possessCard = null;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        await _choice.Initialize();
        await _choice.Close();
        // チョイスメニューのボタン設定
        _choice.SetSelectCallback(async (index) =>
        {
            // そのカードの説明文を出す
            await UIManager.instance.OpenCardText(index);
        });
        _closeButton.gameObject.SetActive(false);
    }

    public void SetPosseessCard(ref PossessCard setPossessCard)
    {
        possessCard = setPossessCard;
    }

    /// <summary>
    /// デッキボタンが押されたとき
    /// </summary>
    public async void OnShowDeck()
    {
        if (possessCard == null) return;
        // 手札の入力終了
        _IsHandAccept = UIManager.instance.IsHandAccept;
        UIManager.instance.EndHandAccept();
        // ボタンを非表示
        _deckDetail.gameObject.SetActive(false);
        _discardAreaDetail.gameObject.SetActive(false);
        _choice.RemoveAllItem();
        // チョイスメニューに設定
        List<int> deck = possessCard.deckCardIDList;
        _choice.SetChoiceCardID(deck);
        List<string> deckText = new List<string>(deck.Count);
        for (int i = 0; i < deck.Count; i++)
        {
            deckText.Add("詳細");
        }
        _choice.SetChoiceButtonText(deckText);

        _closeButton.gameObject.SetActive(true);
        await _choice.Open();
    }

    /// <summary>
    /// 捨て札ボタンが押されたとき
    /// </summary>
    public async void OnShowDiscard()
    {
        if (possessCard == null) return;

        // 手札の入力終了
        _IsHandAccept = UIManager.instance.IsHandAccept;
        UIManager.instance.EndHandAccept();
        // ボタンを非表示
        _deckDetail.gameObject.SetActive(false);
        _discardAreaDetail.gameObject.SetActive(false);
        _choice.RemoveAllItem();

        // チョイスメニューに設定
        List<int> deck = possessCard.discardCardIDList;
        _choice.SetChoiceCardID(deck);
        List<string> deckText = new List<string>(deck.Count);
        for (int i = 0; i < deck.Count; i++)
        {
            deckText.Add("詳細");
        }
        _choice.SetChoiceButtonText(deckText);
        _closeButton.gameObject.SetActive(true);
        await _choice.Open();
    }

    /// <summary>
    /// 詳細画面の終了
    /// </summary>
    public async void OnEndDetail()
    {
        // 手札の入力受付を再開
        if(_IsHandAccept) 
            UIManager.instance.StartHandAccept();

        // ボタンの表示再開
        _deckDetail.gameObject.SetActive(true);
        _discardAreaDetail.gameObject.SetActive(true);
        _closeButton.gameObject.SetActive(false);

        await UIManager.instance.CloseCardText();
        // ウィンドウを閉じる
        await _choice.Close();
    }
}
