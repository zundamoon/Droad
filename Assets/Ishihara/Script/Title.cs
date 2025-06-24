using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] DontDestroy destroy;
    [SerializeField] AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        destroy.CheckInstance();
        if (AudioManager.instance == null) AudioManager.instance = audioManager;
        audioManager.Init();
        AudioManager.instance.PlayBGM(GameEnum.BGM.TITLE);
    }
}
