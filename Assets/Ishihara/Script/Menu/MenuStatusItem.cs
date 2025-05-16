using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class MenuStatusItem : MonoBehaviour
{
    [SerializeField]
    private RectTransform _rectTransform = null;

    public bool isChara { get; protected set; } = false;

    public async virtual UniTask Initialize()
    {
        await UniTask.CompletedTask;
    }

    public float GetHeight()
    {
        return _rectTransform.rect.height;
    }

    public float GetTopPos()
    {
        return _rectTransform.anchoredPosition.y + _rectTransform.rect.height / 2;
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask Move(Vector3 pos, float duration = 0.3f)
    {
        Vector3 startPos = _rectTransform.anchoredPosition;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            _rectTransform.anchoredPosition = Vector3.Lerp(startPos, pos, t);
            await UniTask.Yield();
        }

        _rectTransform.anchoredPosition = pos;
    }

    /// <summary>
    /// サイズ変更
    /// </summary>
    /// <param name="targetScale"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask ReSize(float targetScale, float duration = 0.3f)
    {
        Vector3 startScale = _rectTransform.localScale;
        Vector3 endScale = Vector3.one * targetScale;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            _rectTransform.localScale = Vector3.Lerp(startScale, endScale, t);
            await UniTask.Yield();
        }

        _rectTransform.localScale = endScale;
    }
}
