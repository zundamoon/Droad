using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountUIButton : MonoBehaviour
{
    [SerializeField] public BaseCountUI countUI;
    [SerializeField] public int value = 0;

    /// <summary>
    /// ‰Ÿ‚³‚ê‚½‚Ìˆ—
    /// </summary>
    public void PushButton()
    {
        countUI.AddCount(value);
        AudioManager.instance.PlaySE(GameEnum.SE.SELECT_2);
    }
    
}
