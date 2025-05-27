using System.Collections;
using System.Collections.Generic;
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

    // 手札の入力が受付中かどうか
    private bool _IsHandAccept = false;

    private Character character = null;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        _choice = Instantiate(_choice);
        await _choice.Initialize();
        await _choice.Close();
        // チョイスメニューのボタン設定
        _choice.SetSelectCallback(async (index) =>
        {
            // そのカードの説明文を出す
            await UIManager.instance.OpenCardText(index);
        });
    }

    /// <summary>
    /// デッキボタンが押されたとき
    /// </summary>
    public void OnShowDeck()
    {
        // 手札の入力終了
        _IsHandAccept = UIManager.instance.IsHandAccept;
        UIManager.instance.EndHandAccept();
        // ボタンを非表示
        _deckDetail.gameObject.SetActive(false);
        _discardAreaDetail.gameObject.SetActive(false);

        // デッキを表示

        // どこからターンプレイヤーを取得するのか
        // CardTextをどのタイミングで閉じるのか

        // ターンプレイヤーを取得
        // デッキ取得
        // チョイスメニューに設定
        // 

    }

    /// <summary>
    /// 捨て札ボタンが押されたとき
    /// </summary>
    public void OnShowDiscard()
    {
        // 手札の入力終了
        _IsHandAccept = UIManager.instance.IsHandAccept;
        UIManager.instance.EndHandAccept();
        // ボタンを非表示
        _deckDetail.gameObject.SetActive(false);
        _discardAreaDetail.gameObject.SetActive(false);

        // 捨て札を表示

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

        // ウィンドウを閉じる
        await Close();
    }
}
