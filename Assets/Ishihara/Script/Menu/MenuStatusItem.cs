using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStatusItem : MonoBehaviour
{
    [SerializeField]
    private RectTransform _rectTransform = null;

    public float GetHeight()
    {
        return _rectTransform.rect.height;
    }

    public float GetTopPos()
    {
        return _rectTransform.anchoredPosition.y + _rectTransform.rect.height / 2;
    }

    public async UniTask Move(Vector3 pos)
    {
        _rectTransform.anchoredPosition = pos;
        await UniTask.CompletedTask;
    }
}
