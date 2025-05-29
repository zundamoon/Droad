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
    // 順位
    public int rank { get; private set; } = 0;
    // コイン
    public int coins { get; private set; } = -1;
    // スター
    public int stars { get; private set; } = -1;
    // プレイヤーのID
    public int playerID { get; private set; } = -1;
    // 効果無効フラグ
    public bool eventCancel { get; private set; } = false;
    // マス効果実行回数
    public int eventRepeatCount { get; private set; } = 1;
    // 移動後のイベント
    public Func<List<Character>, UniTask> AfterMoveEvent { get; private set; } = null;
    // 位置情報
    public StagePosition position;
    // 次の移動先を保持
    public StagePosition nextPosition;
    // ステータス更新のコールバック
    public Action<Character> UpdateStatus { get; private set; } = null;
    private float moveSpeed = 10.0f;
    private float goalDistance = 0.05f;
    private const int _NO_COIN_TEXT_ID = 109;

    public void Init(int setPlayerID)
    {
        coins = DEFAULT_COINS;
        stars = DEFAULT_STARS;
        possessCard = new PossessCard();
        possessCard.SetCallback(AddStar, RemoveStar, SetCard);
        possessCard.Init();
        playerID = setPlayerID;
        position.m_route = 0;
        position.m_road = 0;
        position.m_square = 0;
        Vector3 initPosition = StageManager.instance.GetPosition(position);
        SetPosition(initPosition);
        nextPosition = StageManager.instance.GetNextPosition(position);
        AdaptPlayerColor();
        AddStar(possessCard.CountStar());
    }
    /// <summary>
    /// 移動後イベントの設定
    /// </summary>
    /// <param name="setEvent"></param>
    public void SetAfterMoveEvent(Func<List<Character>, UniTask> setEvent) { AfterMoveEvent = setEvent; }

    /// <summary>
    /// 移動後のイベント実行
    /// </summary>
    /// <param name="targetCharacterList"></param>
    public void ExecuteAfterMoveEvent(List<Character> targetCharacterList)
    {
        if (AfterMoveEvent == null) return;
        AfterMoveEvent(targetCharacterList);
        AfterMoveEvent = null;
    }
    
    public void SetUpdateStatus(Action<Character> setCallback)
    {
        UpdateStatus = setCallback;
    }

    public void SetRank(int setRank) { rank = setRank; }
    public void SetCoin(int value) { coins = value; }
    public void AddCoin(int value) 
    { 
        coins += value;
        if (UpdateStatus == null) return;
        UpdateStatus(this);
    }
    public int RemoveCoin(int value)
    {
        int result = Math.Max(0, coins - value);
        int removeCoin = coins - result;
        coins = result;
        if (UpdateStatus == null) return -1;
        UpdateStatus(this);
        return removeCoin;
    }
    public void AddStar(int value) 
    { 
        stars += value;
        if (UpdateStatus == null) return;
        UpdateStatus(this);
    }
    public int RemoveStar(int value)
    {
        int result = Math.Max(0, stars - value);
        int removeStar = stars - result;
        stars = result;
        if (UpdateStatus == null) return -1;
        UpdateStatus(this);
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

    /// <summary>
    /// イベントの繰り返し数設定
    /// </summary>
    /// <param name="count"></param>
    public void SetRepeatEventCount(int count)
    {
        eventRepeatCount = count;
    }

    public int GetStarCount() { return possessCard.CountStar(); }

    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    public void SetCard()
    {
        UIManager.instance.UpdateStatus(this);
    }

    /// <summary>
    /// 移動関数
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public async UniTask Move(Vector3 targetPos, float jumpHeight = 5.0f)
    {
        Vector3 startPos = transform.position;
        float distance = Vector3.Distance(new Vector3(startPos.x, 0, startPos.z), new Vector3(targetPos.x, 0, targetPos.z));
        Vector3 currentPos = startPos;

        while (Vector3.Distance(new Vector3(currentPos.x, 0, currentPos.z), new Vector3(targetPos.x, 0, targetPos.z)) > goalDistance)
        {
            // 横移動
            Vector3 nextPos = Vector3.MoveTowards(new Vector3(currentPos.x, 0, currentPos.z), new Vector3(targetPos.x, 0, targetPos.z), moveSpeed * Time.deltaTime);
            float traveled = Vector3.Distance(new Vector3(startPos.x, 0, startPos.z), nextPos);
            float t = traveled / distance;

            // 高さ計算（Sin波 + Lerp）
            float heightOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            float y = Mathf.Lerp(startPos.y, targetPos.y, t) + heightOffset;

            // 縦と横を合成する
            currentPos = new Vector3(nextPos.x, y, nextPos.z);
            transform.position = currentPos;

            await UniTask.Yield();
        }

        transform.position = targetPos; // 最終位置補正
    }

    public async UniTask SquareStand()
    {
        // 立っているマスを取得
        Square square = StageManager.instance.GetSquare(position);
        // 立つべきマスの座標を管理
        Vector3 standPos = StageManager.instance.GetPosition(position);
        // 移動
        await Move(standPos, 3);
    }

    /// <summary>
    /// マスからずれた位置に立つ
    /// </summary>
    /// <returns></returns>
    public async UniTask SquareShift()
    {
        // 立っているマスを取得
        Square square = StageManager.instance.GetSquare(position);
        // 立つべきマスの座標を管理
        Vector3 standPos = StageManager.instance.GetPosition(position);

        for (int i = 0; i < square.standingPlayerList.Count; i++)
        {
            if (square.standingPlayerList[i] == playerID)
            {
                standPos = square.standAnchorList[i].gameObject.transform.position;
                break;
            }
        }
        await Move(standPos, 3);
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
    /// <summary>
    /// 支払いをする
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public async UniTask<bool> Pay(int price)
    {
        bool result = coins >= price;
        if (result) RemoveCoin(price);
        else await UIManager.instance.RunMessage(_NO_COIN_TEXT_ID.ToText());

        return result;
    }
}
