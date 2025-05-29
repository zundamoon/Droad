using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private TextMeshProUGUI _advanceText = null;
    [SerializeField]
    private TextMeshProUGUI _coinText = null;
    [SerializeField]
    private TextMeshProUGUI _eventText = null;
    [SerializeField]
    private TextMeshProUGUI _cardNameText = null;
    [SerializeField]
    private Image _BGFrame = null;
    [SerializeField]
    private Image _cardImage = null;
    [SerializeField]
    private GameObject _highLight = null;

    [SerializeField]
    private Color _cardBronzeMaterial = Color.white;
    [SerializeField]
    private Color _cardSilverMaterial = Color.white;
    [SerializeField]
    private Color _cardGoldMaterial = Color.white;
    [SerializeField]
    private Color _cardLegendaryMaterial = Color.white;
    [SerializeField]
    private Color _cardStarMaterial = Color.white;

    private int _ID = -1;
    private Transform _handArea;
    private int _handIndex = -1;

    private Action<int> _OnUseCard = null;

    public void OnEnable()
    {
        _highLight.SetActive(false);
    }

    /// <summary>
    /// カーソルがあっているとき
    /// </summary>
    public void OnPointEnter()
    {
        if (!UIManager.instance.IsHandAccept) return;

        _highLight.SetActive(true);
    }

    /// <summary>
    /// カーソルが外れた時
    /// </summary>
    public void OnPointExit()
    {
        if (!UIManager.instance.IsHandAccept) return;

        _highLight.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!UIManager.instance.IsHandAccept) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Transform field = UIManager.instance.GetHandCanvas().transform;
            // ドラッグしたオブジェクトを親から外す
            _handArea = transform.parent;
            transform.SetParent(field);
            // 大きくする
            transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!UIManager.instance.IsHandAccept) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector3 mousePos = Input.mousePosition;

            // 移動量から傾き方向を計算（画面座標でOK）
            Vector3 move = mousePos - transform.position;

            float tiltFactor = 1;
            float maxTilt = 20f;

            float tiltX = Mathf.Clamp(move.y * tiltFactor, -maxTilt, maxTilt);
            float tiltY = Mathf.Clamp(-move.x * tiltFactor, -maxTilt, maxTilt);

            // 傾ける
            transform.localRotation = Quaternion.Euler(tiltX, tiltY, 0);

            // マウス位置に追従（スクリーン座標ベースでOK）
            transform.position = mousePos;
        }
    }

    public async void OnEndDrag(PointerEventData eventData)
    {
        if (!UIManager.instance.IsHandAccept) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 元のサイズに戻す
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            transform.localRotation = Quaternion.identity;

            // 使用エリアなら
            if (!UIManager.instance.CheckPlayArea(Input.mousePosition))
            {
                // 使用エリア外なら元の位置に戻す
                transform.SetParent(_handArea);
                return;
            }

            await UseCard();
        }
    }
    private async UniTask UseCard()
    {
        // 入力受付終了
        UIManager.instance.EndHandAccept();
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        float duration = 0.5f;
        float elapsed = 0f;

        // 最初に裏向きにする（Y軸180度）
        transform.localRotation = Quaternion.Euler(0, 180f, 0);

        // 表向きに回転しながら中央へ移動
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime * 2;
            float t = Mathf.Clamp01(elapsed / duration);

            // 裏向きから表向きへ
            float yRot = Mathf.Lerp(180f, 0f, t);
            transform.localRotation = Quaternion.Euler(0, yRot, 0);

            // 位置も中央へ補間
            transform.position = Vector3.Lerp(startPos, endPos, t);

            await UniTask.Yield();
        }
        // 待つ
        await UniTask.Delay(200);

        // 消してエフェクト再生
        gameObject.SetActive(false);
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        // スクリーン上に再生
        await EffectManager.instance.CreateScreenEffect(0, endPos, Quaternion.identity);
        // 使用カードをターンに通知
        _OnUseCard(_handIndex);
    }

    /// <summary>
    /// カード情報を設定
    /// </summary>
    /// <param name="setID"></param>
    public void SetCard(int setID)
    {
        _ID = setID;
        // カード情報取得
        CardData card = CardManager.GetCard(_ID);
        if (card == null) return;

        _cardNameText.text = card.nameID.ToText();
        _advanceText.text = card.advance.ToString();
        _coinText.text = card.addCoin.ToString();
        switch(card.rarity)
        {
            case GameEnum.Rarity.BRONZE:
                _BGFrame.color = _cardBronzeMaterial;
                break;
            case GameEnum.Rarity.SILVER:
                _BGFrame.color = _cardSilverMaterial;
                break;
            case GameEnum.Rarity.GOLD:
                _BGFrame.color = _cardGoldMaterial;
                break;
            case GameEnum.Rarity.LEGENDARY:
                _BGFrame.color = _cardLegendaryMaterial;
                break;
            case GameEnum.Rarity.STAR:
                _BGFrame.color = _cardStarMaterial;
                break;
        }
        Entity_EventData.Param param = EventMasterUtility.GetEventMaster(card.eventID);
        if (param == null)
        {
            _eventText.text = "";
            return;
        }
        int eventTextID = param.textID;
        int[] paramList = param.param;
        _eventText.text = string.Format(eventTextID.ToText(), paramList[0], paramList[1]);
    }

    /// <summary>
    /// 表示番号を設定
    /// </summary>
    /// <param name="index"></param>
    public void SetHandIndex(int index)
    {
        _handIndex = index;
    }

    /// <summary>
    /// カードを使った際のコールバック設定
    /// </summary>
    /// <param name="setCallback"></param>
    public void SetOnUseCard(Action<int> setCallback)
    {
        _OnUseCard = setCallback;
    }
    /// <summary>
    /// 選択されたとき
    /// </summary>
    public void OnSelect()
    {
        if (!UIManager.instance.IsHandAccept) return;
        UIManager.instance.OpenCardText(_ID);
    }

    public void OnDeselect()
    {
        if (!UIManager.instance.IsHandAccept) return;
        UIManager.instance.CloseCardText();
    }
}