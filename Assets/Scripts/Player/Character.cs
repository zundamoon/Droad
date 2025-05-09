using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // 所持カード
    public PossessCard possessCard { get; private set; } = null;
    // コイン
    public int coins { get; private set; } = -1;
    // スター
    public int stars { get; private set; } = -1;
    // 効果無効フラグ
    public bool eventCancel { get; private set; } = false;
    // 移動後のイベント
    private Action<List<Character>> _AfterMoveEvent = null;

    public void Init()
    {
        possessCard = new PossessCard();
        possessCard.Init();
    }
    /// <summary>
    /// 移動後イベントの設定
    /// </summary>
    /// <param name="setEvent"></param>
    public void SetAfterMoveEvent(Action<List<Character>> setEvent) { _AfterMoveEvent = setEvent; }
    public void ExecuteAfterMoveEvent(List<Character> targetCharacterList)
    {
        if (_AfterMoveEvent == null) return;
        _AfterMoveEvent(targetCharacterList);
        _AfterMoveEvent = null;
    }

    public void SetCoin(int value) { coins = value; }
    public void AddCoin(int value) { coins += value; }
    public int RemoveCoin(int value)
    {
        int removeCoin = Math.Max(0, coins - value);
        coins -= removeCoin;
        return removeCoin;
    }
    public void AddStar(int value) {  stars += value; }
    public int RemoveStar(int value)
    {
        int removeStar = Math.Max(0, stars - value);
        stars -= removeStar;
        return removeStar;
    }
    public void SetCancelEvent() { eventCancel = true; }
    /// <summary>
    /// イベントを実行できるか
    /// </summary>
    /// <returns></returns>
    public bool CanEvent()
    {
        if (eventCancel)
        {
            eventCancel = false;
            return true;
        }
        return false;
    }
}
