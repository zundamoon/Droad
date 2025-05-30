using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class FadeSceneChange : MonoBehaviour
{
    public static FadeSceneChange instance = null;
    [SerializeField] private bool cursorLock = true;

    private const float DEFAULT_DELAY_SECOND = 0;
    private const float DEFAULT_FADE_SECOND = 1.0f;
    private void Start()
    {
        instance = this;
        SetCursorLock(cursorLock);
    }

    public void SetCursorLock(bool isLock)
    {
        if (isLock)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public static void ChangeSceneEvent(string sceneName)
    {
        _ = ChangeScene(sceneName);
    }

    public void EndGameEvent()
    {
        _ = EndGame();
    }

    private static async UniTask ChangeScene(string sceneName, float sec = DEFAULT_DELAY_SECOND)
    {
    
        await FadeScreen.instance.FadeOut(DEFAULT_FADE_SECOND);
        await Task.Delay(TimeSpan.FromSeconds(sec));
        SceneManager.LoadScene(sceneName);
        await FadeScreen.instance.FadeIn(DEFAULT_FADE_SECOND);
    }

    private async UniTask EndGame(float sec = DEFAULT_DELAY_SECOND)
    {
        await FadeScreen.instance.FadeOut();
        await Task.Delay(TimeSpan.FromSeconds(sec));
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }

    public static void NoneFadeChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
