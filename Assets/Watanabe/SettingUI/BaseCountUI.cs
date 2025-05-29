using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseCountUI : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI countText;
    protected int count = 1;

    public void Init()
    {
        countText.text = count.ToString();
    }

    public int GetCount() { return count; }
    public virtual void AddCount(int value)
    {
        count += value;
        countText.text = count.ToString();
    }
    
}
