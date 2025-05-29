/*
 * @file FadeScreen.cs
 * @brief �t�F�[�h�C���E�t�F�[�h�A�E�g������
 * @author sein
 * @date 2025/1/17
 */

using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private UnityEngine.UI.Image fade;            // �����

    [SerializeField] public float alphaValue = 0.0f;               // Fade�C���[�W�̃A���t�@�l���Ǘ�

    private readonly float ALPHA_VALUE_MAX = 1.0f;     // �A���t�@�l�̍ő�l
    private readonly float ALPHA_VALUE_MIN = 0.0f;     // �A���t�@�l�̍ŏ��l
    private readonly float DEFAULT_FADE_SPEED = 5.0f;


    public static FadeScreen instance { get; private set; } = null;

    private void Start()
    {
        instance = this;
        // canvas = UIManager.instance.canvas.GetComponent<Canvas>();
    }

    /// <summary>
    /// �t�F�[�h�p��Canvas�𐶐�����
    /// </summary>
    public void GenerateFadeScreen()
    {

    }

    /// <summary>
    /// ����ʂ��������鏈��
    /// </summary>
    /// <returns></returns>
    public async UniTask FadeIn(float fadeInSpeed = 5.0f)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInSpeed)
        {
            float delta = Time.deltaTime / fadeInSpeed;
            alphaValue = Mathf.Clamp01(alphaValue - delta);
            fade.color = new Color(0.0f, 0.0f, 0.0f, alphaValue);

            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }

        // �Ō�ɂ������� ALPHA_VALUE_MIN �ɐݒ�
        alphaValue = ALPHA_VALUE_MIN;
        fade.color = new Color(0.0f, 0.0f, 0.0f, alphaValue);
        canvas.sortingOrder = -100;
    }

    /// <summary>
    /// ����ʂɂ��鏈��
    /// </summary>
    /// <returns></returns>
    public async UniTask FadeOut(float fadeOutSpeed = 2.5f)
    {
        float elapsedTime = 0f;
        canvas.sortingOrder = 3;
        while (elapsedTime < fadeOutSpeed)
        {
            float delta = Time.deltaTime / fadeOutSpeed;
            alphaValue = Mathf.Clamp01(alphaValue + delta);
            fade.color = new Color(0.0f, 0.0f, 0.0f, alphaValue);

            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }

        // �Ō�ɂ������� ALPHA_VALUE_MAX �ɐݒ�
        alphaValue = ALPHA_VALUE_MAX;
        fade.color = new Color(0.0f, 0.0f, 0.0f, alphaValue);
    }

    public async UniTask FadeInterval(float sec)
    {
        await FadeScreen.instance.FadeOut(DEFAULT_FADE_SPEED);
        await Task.Delay(TimeSpan.FromSeconds(sec));
        await FadeScreen.instance.FadeIn(DEFAULT_FADE_SPEED);
    }
}
