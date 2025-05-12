using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

using static CommonModule;

public class Character : MonoBehaviour
{
    // 自身
    public GameObject playerObject = null;
    // 所持カード
    public PossessCard possessCard { get; private set; } = null;
    // コイン
    public int coins { get; private set; } = -1;
    // スター
    public int stars { get; private set; } = -1;
    // 効果無効フラグ
    public bool eventCancel { get; private set; } = false;
    // 移動後のイベント
    public Action<List<Character>> AfterMoveEvent { get; private set; } = null;

    // 位置情報
    public StagePosition position;
    // 次の移動先を保持
    public StagePosition nextPosition;

    public float moveSpeed = 3.0f;

    public float goalDistance = 0.05f;

    public void Init()
    {
        playerObject = GetComponent<GameObject>();
        possessCard = new PossessCard();
        possessCard.Init();
        position.route = 0;
        position.road = 0;
        position.square = 0;
    }
    /// <summary>
    /// 移動後イベントの設定
    /// </summary>
    /// <param name="setEvent"></param>
    public void SetAfterMoveEvent(Action<List<Character>> setEvent) { AfterMoveEvent = setEvent; }
    public void ExecuteAfterMoveEvent(List<Character> targetCharacterList)
    {
        if (AfterMoveEvent == null) return;
        AfterMoveEvent(targetCharacterList);
        AfterMoveEvent = null;
    }

    public void SetCoin(int value) { coins = value; }
    public void AddCoin(int value) { coins += value; }
    public int RemoveCoin(int value)
    {
        int removeCoin = Math.Max(0, coins - value);
        coins -= removeCoin;
        return removeCoin;
    }
    public void AddStar(int value) { stars += value; }
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
    /// <summary>
    /// 移動関数
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public async UniTask Move(Vector3 targetPos)
    {
        // 移動ループ
        while (Vector3.Distance(playerObject.transform.position, targetPos) > goalDistance)
        {
            // 補間で滑らかに移動
            transform.position = Vector3.Lerp(playerObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
            await UniTask.DelayFrame(1);
        }
        transform.position = targetPos;
    }
}
