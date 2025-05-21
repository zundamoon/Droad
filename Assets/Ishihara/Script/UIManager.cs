using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : SystemObject
{
    public static UIManager instance { get; private set; } = null;

    [SerializeField]
    private MenuChoice _menuChoice = null;

    [SerializeField]
    private MenuHand _menuHand = null;

    [SerializeField]
    private MessageUI _messageUI = null;

    [SerializeField]
    private MenuStatus _menuStatus = null;

    [SerializeField]
    private MenuShop _menuShop = null;

    private UniTaskCompletionSource _uniTaskCompletionSource = null;

    private const float _DEFAULT_DISPLAY_TIME = 0.75f;

    public bool IsHandAccept { get; private set; } = false;

    public async override UniTask Initialize()
    {
        await base.Initialize();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // メニューを生成
        _menuHand = Instantiate(_menuHand);
        _menuChoice = Instantiate(_menuChoice);
        _messageUI = Instantiate(_messageUI);
        _menuStatus = Instantiate(_menuStatus);
        _menuShop = Instantiate(_menuShop);

        await _menuHand.Initialize();
        await _menuChoice.Initialize();
        await _menuStatus.Initialize();
        await _menuShop.Initialize();
        _menuStatus.SetCharacter(CharacterManager.instance.GetAllCharacter());

        await _menuHand.Close();
        await _menuChoice.Close();
        await _messageUI.Inactive();
        _menuShop.Open();
        _menuShop.Close();
    }

    public GameObject GetHandCanvas()
    {
        return _menuHand.GetCanvas();
    }

    /// <summary>
    /// 使用エリアかどうか
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckPlayArea(Vector2 pos)
    {
        RectTransform playArea = _menuHand.GetPlayArea();
        if (playArea == null)
        {
            return false;
        }

        Vector2 localPos;
        // スクリーン座標をローカル座標に変換
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(playArea, pos, null, out localPos))
        {
            return false;
        }

        // 範囲内かどうか判定
        return playArea.rect.Contains(localPos);
    }

    /// <summary>
    /// 手札エリアを開く
    /// </summary>
    /// <returns></returns>
    public async UniTask OpenHandArea(PossessCard setPossessCard)
    {
        _menuHand.SetTurnPlayerCard(setPossessCard);
        await _menuHand.Open();
    }

    /// <summary>
    /// 手札エリアを閉じる
    /// </summary>
    /// <returns></returns>
    public async UniTask CloseHandArea()
    {
        await _menuHand.Close();
    }

    /// <summary>
    /// 選択肢エリアを開く
    /// </summary>
    /// <returns></returns>
    public async UniTask OpenChoiceArea(List<int> choiceCardIDList)
    {
        _uniTaskCompletionSource = new UniTaskCompletionSource();

        _menuChoice.SetChoiceCardID(choiceCardIDList);
        await _menuChoice.Open();

        await _uniTaskCompletionSource.Task;
    }

    public async UniTask SetChoiceCallback(System.Action<int> action)
    {
        _menuChoice.SetSelectCallback(async (index) =>
        {
            action(index);
            _uniTaskCompletionSource.TrySetResult();
            await CloseChoiceArea();
        });
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 選択肢エリアを閉じる
    /// </summary>
    /// <returns></returns>
    public async UniTask CloseChoiceArea()
    {
        _menuChoice.RemoveAllItem();
        await _menuChoice.Close();
    }

    /// <summary>
    /// 手札入力受付開始
    /// </summary>
    public void StartHandAccept()
    {
        IsHandAccept = true;
    }

    /// <summary>
    /// 手札入力受付終了
    /// </summary>
    public void EndHandAccept()
    {
        IsHandAccept = false;
    }

    /// <summary>
    /// メッセージUIを表示する
    /// </summary>
    /// <param name="text"></param>
    /// <param name="displayTime"></param>
    /// <returns></returns>
    public async UniTask RunMessage(string text, float displayTime = _DEFAULT_DISPLAY_TIME)
    {
        await _messageUI.RunMessage(text, displayTime);
    }

    /// <summary>
    /// ステータスエリアに追加する
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public async UniTask AddStatus(int CharacterID)
    {
        Character character = CharacterManager.instance.GetCharacter(CharacterID);
        await _menuStatus.AddStatus(character);
    }

    /// <summary>
    /// ステータスエリアに追加する
    /// </summary>
    /// <returns></returns>
    public async UniTask AddStatus(string str)
    {
        await _menuStatus.AddStatus(str);
    }

    /// <summary>
    /// ステータスエリアに追加する
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public async UniTask AddStatus(List<int> CharacterID)
    {
        for (int i = 0; i < CharacterID.Count; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(CharacterID[i]);
            await _menuStatus.AddStatus(character);
        }
    }

    /// <summary>
    /// ステータスエリアをスクロールする
    /// </summary>
    /// <returns></returns>
    public async UniTask ScrollStatus()
    {
        await _menuStatus.ScrollStatus();
    }

    /// <summary>
    /// 先頭のステータスを大きくする
    /// </summary>
    /// <returns></returns>
    public async UniTask ReSizeTop()
    {
        await _menuStatus.ReSizeTop();
    }

    /// <summary>
    /// ステータスエリアをクリアする
    /// </summary>
    /// <returns></returns>
    public async UniTask ClearStatus()
    {
        await _menuStatus.RemoveAllStatus();
    }

    /// <summary>
    /// 全てのステータスをスクロールする
    /// </summary>
    /// <returns></returns>
    public async UniTask ScrollAllStatus()
    {
        await _menuStatus.ScrollAllStatus();
    }

    /// <summary>
    /// ショップを開く
    /// </summary>
    /// <returns></returns>
    public async UniTask OpenShop()
    {
        await _menuShop.Open();
    }

    /// <summary>
    /// ショップを閉じる
    /// </summary>
    /// <returns></returns>
    public async UniTask CloseShop()
    {
        _menuShop.Close();
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 購入アイテムの設定
    /// </summary>
    /// <param name="itemIDList"></param>
    /// <returns></returns>
    public async UniTask SetBuyItem(List<int> itemIDList)
    {
        _menuShop.SetBuyCardID(itemIDList);
        await UniTask.CompletedTask;

    }

    /// <summary>
    /// 除外アイテムの設定
    /// </summary>
    /// <param name="itemIDList"></param>
    /// <returns></returns>
    public async UniTask SetRemovaItem(List<int> itemIDList)
    {
        _menuShop.SetRemovalCardID(itemIDList);
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 購入処理コールバックの設定
    /// </summary>
    /// <param name="onSelect"></param>
    /// <returns></returns>
    public async UniTask SetSelectCallback(System.Action<int, bool> onSelect)
    {
        await _menuShop.SetSelectCallback(onSelect);
    }

    /// <summary>
    /// ショップのアイテム削除
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="isRemove"></param>
    /// <returns></returns>
    public async UniTask RemoveShopItem(int cardID, bool isRemove = false)
    {
        await _menuShop.RemoveShopItem(cardID, isRemove);
    }

    /// <summary>
    /// カード使用の設定
    /// </summary>
    /// <returns></returns>
    public async UniTask SetOnUseCard(System.Action<int> action)
    {
        _menuHand.SetOnUseCard(action);
        await UniTask.CompletedTask;
    }
}
