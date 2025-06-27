using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    // メッセージ
    [SerializeField]
    private GameObject _messageObject = null;
    // 表示アンカー
    [SerializeField]
    private RectTransform _messageDisplayAnchor = null;

    // バナー
    [SerializeField]
    private GameObject _bannerObject = null;
    // 表示アンカー
    [SerializeField]
    private RectTransform _bannerDisplayAnchor = null;

    private TextMeshProUGUI _messageText = null;
    private Image _messageTextBG = null;
    private TextMeshProUGUI _bannerText = null;
    private Image _bannerTextBG = null;

    private Color _defaultTextColor = Color.white;
    private Color _defaultBGColor = Color.white;

    private const float _MOVE_TIME = 0.25f;
    private const float _MOVE_HEIGHT = 100;

    public async UniTask Initialize()
    {
        _messageText = _messageObject.GetComponentInChildren<TextMeshProUGUI>();
        _messageTextBG = _messageObject.GetComponent<Image>();
        _bannerText = _bannerObject.GetComponentInChildren<TextMeshProUGUI>();
        _bannerTextBG = _bannerObject.gameObject.GetComponent<Image>();
        _defaultBGColor = _messageTextBG.color;
        _defaultTextColor = _messageText.color;
        _messageObject.SetActive(false);
        _bannerObject.SetActive(false);
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// メッセージを流す
    /// </summary>
    /// <param name="displayTime"></param>
    /// <returns></returns>
    public async UniTask RunMessage(string setText, float displayTime)
    {
        _messageObject.SetActive(true);
        // テキストの表示
        _messageText.text = setText;
        _messageTextBG.rectTransform.anchoredPosition = _messageDisplayAnchor.anchoredPosition;

        float elapsedTime = 0;
        // 移動
        while (elapsedTime < _MOVE_TIME)
        {
            // UIを動かす
            float ratio = elapsedTime / _MOVE_TIME;
            _messageTextBG.rectTransform.anchoredPosition += new Vector2(0, _MOVE_HEIGHT / _MOVE_TIME * Time.deltaTime);
            SetMessageAlpha(ratio);
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        _messageTextBG.rectTransform.anchoredPosition = _messageDisplayAnchor.anchoredPosition + new Vector2(0, _MOVE_HEIGHT);
        SetMessageAlpha(1);
        elapsedTime = 0;
        // 待機
        while (elapsedTime < displayTime)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        _messageObject.SetActive(false);
    }

    /// <summary>
    /// メッセージのアルファを設定
    /// </summary>
    /// <param name="alpha"></param>
    private void SetMessageAlpha(float alpha)
    {
        // テキストの透過度設定
        Color color = _defaultTextColor;
        color.a = _defaultTextColor.a * alpha;
        _messageText.color = color;
        // BGの透過設定
        color = _defaultBGColor;
        color.a = _defaultBGColor.a * alpha;
        _messageTextBG.color = color;
    }

    public async UniTask RunBanner(string setText)
    {
        _bannerObject.SetActive(true);
        // テキストの表示
        _bannerText.text = setText;
        _bannerTextBG.rectTransform.anchoredPosition = _bannerDisplayAnchor.anchoredPosition;
    }

    public async UniTask CloseBanner()
    {
        _bannerObject.SetActive(false);
    }
}
