using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/*
 * Ishihara
 * �}�l�[�W���[�N���X�̊��
 */
public class SystemObject : MonoBehaviour
{
    /// <summary>
    /// ������
    /// </summary>
    public async virtual UniTask Initialize()
    {
        // ����������
        await UniTask.CompletedTask;
    }
}
