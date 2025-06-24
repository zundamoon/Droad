using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] DontDestroy destroy;

    // Start is called before the first frame update
    void Start()
    {
        destroy.CheckInstance();
        AudioManager.instance.PlayBGM(GameEnum.BGM.TITLE);
    }
}
