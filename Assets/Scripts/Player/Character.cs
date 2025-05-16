using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

using static CommonModule;
using static GameConst;

public class Character : MonoBehaviour
{
    // カメラアンカー
    [SerializeField] private Transform _cameraAnchor = null;
    // 色変更のために保持
    [SerializeField] private new Renderer renderer = null;
    // プレイヤーのカラー
    public Color playerColor { get; private set; }
    // 所持カード
    public PossessCard possessCard { get; private set; } = null;
    // コイン
    public int coins { get; private set; } = -1;
    // スター
    public int stars { get; private set; } = -1;
    // プレイヤーのID
    public int playerID { get; private set; } = -1;
    // 効果無効フラグ
    public bool eventCancel { get; private set; } = false;
    // 移動後のイベント
    public Action<List<Character>> AfterMoveEvent { get; private set; } = null;
    // 位置情報
    public StagePosition position;
    // 次の移動先を保持
    public StagePosition nextPosition;
    private float moveSpeed = 10.0f;
    private float goalDistance = 0.05f;

    public void Init(int setPlayerID)
    {
        possessCard = new PossessCard();
        possessCard.Init();
        playerID = setPlayerID;
        position.m_route = 0;
        position.m_road = 0;
        position.m_square = 0;
        Vector3 initPosition = StageManager.instance.GetPosition(position);
        SetPosition(initPosition);
        AdaptPlayerColor();

        coins = DEFAULT_COINS;
        stars = DEFAULT_STARS;
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
            return false;
        }
        return true;
    }

    public int GetStarCount() { return possessCard.CountStar(); }

    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    /// <summary>
    /// 移動関数
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public async UniTask Move(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > goalDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            await UniTask.DelayFrame(1);
        }

        transform.position = targetPos;
    }

    /// <summary>
    /// カメラアンカーの取得
    /// </summary>
    /// <returns></returns>
    public Transform GetCameraAnchor()
    {
        return _cameraAnchor;
    }
    public void SetPlayerColor(Color color) { playerColor = color; }
    public void AdaptPlayerColor()
    {
        if (renderer != null)
        {
            renderer.material.color = playerColor;
        }
    }
}
