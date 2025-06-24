using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private Image _displayImage;
    [SerializeField]
    private Sprite[] _tutorialImageList;

    private int currentDisplayImageCount = 0;

    /// <summary>
    /// ŠJ‚­
    /// </summary>
    public void Start()
    {
        _displayImage.sprite = _tutorialImageList[0];
    }

    /// <summary>
    /// •Â‚¶‚é
    /// </summary>
    public void CloseTutorial()
    {
        gameObject.SetActive(false);
    }

    public void OpenTutorial()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ŽŸ‚Ì‰æ‘œ‚É•Ï‚¦‚é
    /// </summary>
    public void NextTutrial()
    {
        currentDisplayImageCount++;
        if (currentDisplayImageCount >= _tutorialImageList.Length)
            currentDisplayImageCount = 0;
        _displayImage.sprite = _tutorialImageList[currentDisplayImageCount];
    }

    /// <summary>
    /// ‘O‚Ì‰æ‘œ‚É•Ï‚¦‚é
    /// </summary>
    public void BackTutorial()
    {
        currentDisplayImageCount--;
        if (currentDisplayImageCount < 0)
            currentDisplayImageCount = _tutorialImageList.Length - 1;
        _displayImage.sprite = _tutorialImageList[currentDisplayImageCount];
    }
}
