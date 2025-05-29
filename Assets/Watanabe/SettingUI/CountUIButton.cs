using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountUIButton : MonoBehaviour
{
    [SerializeField] public BaseCountUI countUI;
    [SerializeField] public int value = 0;

    /// <summary>
    /// �����ꂽ���̏���
    /// </summary>
    public void PushButton()
    {
        countUI.AddCount(value);
    }
    
}
