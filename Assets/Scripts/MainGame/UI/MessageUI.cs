using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    // テキスト
    [SerializeField]
    private TextMeshProUGUI _text = null;
    // テキストBG
    [SerializeField]
    private Image _textBG = null;
    // 表示アンカー
    [SerializeField]
    private RectTransform _displayAnchor = null;

    private const float _MOVE_TIME = 0.25f;
    private const float _MOVE_HEIGHT = 100;

    /// <summary>
    /// メッセージを流す
    /// </summary>
    /// <param name="displayTime"></param>
    /// <returns></returns>
    public async UniTask RunMessage(string setText, float displayTime)
    {
        gameObject.SetActive(true);
        // テキストの表示
        _text.text = setText;
        _textBG.rectTransform.anchoredPosition = _displayAnchor.anchoredPosition;

        float elapsedTime = 0;
        // 移動
        while (elapsedTime < _MOVE_TIME)
        {
            // UIを動かす
            float ratio = elapsedTime / _MOVE_TIME;
            _textBG.rectTransform.anchoredPosition += new Vector2(0, _MOVE_HEIGHT / _MOVE_TIME * Time.deltaTime);
            SetMessageAlpha(ratio);
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        elapsedTime = 0;
        // 待機
        while (elapsedTime < displayTime)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        await Inactive();
    }

    /// <summary>
    /// メッセージのアルファを設定
    /// </summary>
    /// <param name="alpha"></param>
    private void SetMessageAlpha(float alpha)
    {
        // テキストの透過度設定
        Color color = _text.color;
        color.a = alpha;
        _text.color = color;
        // BGの透過設定
        color = _textBG.color;
        color.a = alpha;
        _textBG.color = color;
    }

    /// <summary>
    /// 非表示にする
    /// </summary>
    /// <returns></returns>
    public async UniTask Inactive()
    {
        gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
}
