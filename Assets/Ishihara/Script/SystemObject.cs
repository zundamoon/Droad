using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/*
 * Ishihara
 * マネージャークラスの基底
 */
public class SystemObject : MonoBehaviour
{
    /// <summary>
    /// 初期化
    /// </summary>
    public async virtual UniTask Initialize()
    {
        // 初期化処理
        await UniTask.CompletedTask;
    }
}
