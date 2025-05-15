using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    // �e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _text = null;
    // �e�L�X�gBG
    [SerializeField]
    private Image _textBG = null;
    // �\���A���J�[
    [SerializeField]
    private RectTransform _displayAnchor = null;

    private const float _MOVE_TIME = 0.25f;
    private const float _MOVE_HEIGHT = 100;

    /// <summary>
    /// ���b�Z�[�W�𗬂�
    /// </summary>
    /// <param name="displayTime"></param>
    /// <returns></returns>
    public async UniTask RunMessage(string setText, float displayTime)
    {
        gameObject.SetActive(true);
        // �e�L�X�g�̕\��
        _text.text = setText;
        _text.rectTransform.anchoredPosition = _displayAnchor.anchoredPosition;

        float elapsedTime = 0;
        // �ړ�
        while (elapsedTime < _MOVE_TIME)
        {
            // UI�𓮂���
            float ratio = elapsedTime / _MOVE_TIME;
            _text.rectTransform.anchoredPosition += new Vector2(0, _MOVE_HEIGHT / _MOVE_TIME * Time.deltaTime);
            SetMessageAlpha(ratio);
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        elapsedTime = 0;
        // �ҋ@
        while (elapsedTime < displayTime)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        await Inactive();
    }

    /// <summary>
    /// ���b�Z�[�W�̃A���t�@��ݒ�
    /// </summary>
    /// <param name="alpha"></param>
    private void SetMessageAlpha(float alpha)
    {
        // �e�L�X�g�̓��ߓx�ݒ�
        Color color = _text.color;
        color.a = alpha;
        _text.color = color;
        // BG�̓��ߐݒ�
        color = _textBG.color;
        color.a = alpha;
        _textBG.color = color;
    }

    /// <summary>
    /// ��\���ɂ���
    /// </summary>
    /// <returns></returns>
    public async UniTask Inactive()
    {
        gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
}
