using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } = null;

    [SerializeField]
    private MenuChoice menuChoice;

    [SerializeField]
    private MenuHand menuHand;

    public bool IsHandAccept { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        menuHand.Initialize().Forget();
    }

    public GameObject GetHandCanvas()
    {
        return menuHand.GetCanvas();
    }

    /// <summary>
    /// 使用エリアかどうか
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckPlayArea(Vector2 pos)
    {
        RectTransform playArea = menuHand.GetPlayArea();
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
        menuHand.SetTurnPlayerCard(setPossessCard);
        await menuHand.Open();
    }

    /// <summary>
    /// 手札エリアを閉じる
    /// </summary>
    /// <returns></returns>
    public async UniTask CloseHandArea()
    {
        await menuHand.Close();
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
