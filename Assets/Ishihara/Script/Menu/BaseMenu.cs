using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMenu : MonoBehaviour
{
    // メニューのルートオブジェクト
    [SerializeField]
    private GameObject _menuRoot = null;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Initialize()
    {
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// メニューを開く
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Open()
    {
        _menuRoot.SetActive(true);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// メニューを閉じる
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Close()
    {
        _menuRoot.SetActive(false);
        await UniTask.CompletedTask;
    }

    public GameObject GetCanvas()
    {
        return _menuRoot;
    }
}
