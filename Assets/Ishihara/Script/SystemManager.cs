using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    [SerializeField]
    private List<SystemObject> _systemObject = null;

    private void Start()
    {
        // システムオブジェクトの初期化
        for (int i = 0, max = _systemObject.Count; i < max; i++)
        {
            _systemObject[i].Initialize();
        }
    }
}
