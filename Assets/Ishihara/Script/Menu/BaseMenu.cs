using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMenu : MonoBehaviour
{
    // ���j���[�̃��[�g�I�u�W�F�N�g
    [SerializeField]
    private GameObject _menuRoot = null;

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Initialize()
    {
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// ���j���[���J��
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Open()
    {
        _menuRoot.SetActive(true);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// ���j���[�����
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Close()
    {
        _menuRoot.SetActive(false);
        await UniTask.CompletedTask;
    }
}
