using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : SystemObject
{
    public static UIManager Instance { get; private set; } = null;

    [SerializeField]
    private MenuChoice _menuChoice;

    [SerializeField]
    private MenuHand _menuHand;

    public bool IsHandAccept { get; private set; } = false;

    public async override UniTask Initialize()
    {
        await base.Initialize();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // メニューを生成
        _menuHand = Instantiate(_menuHand);
        _menuChoice = Instantiate(_menuChoice);

        await _menuHand.Initialize();
        await _menuChoice.Initialize();

        await _menuHand.Close();
        await _menuChoice.Close();
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
        _menuChoice.SetChoiceCardID(choiceCardIDList);
        await _menuChoice.Open();
    }

    /// <summary>
    /// 選択肢エリアを閉じる
    /// </summary>
    /// <returns></returns>
    public async UniTask CloseChoiceArea()
    {
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
}
