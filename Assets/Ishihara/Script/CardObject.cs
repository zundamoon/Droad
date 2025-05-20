using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _advanceText = null;
    [SerializeField]
    private TextMeshProUGUI _coinText = null;
    [SerializeField]
    private TextMeshProUGUI _eventText = null;
    [SerializeField]
    private GameObject _highLight = null;

    private int _ID = -1;
    private Transform _handArea;
    private int _handIndex = -1;

    private Action<int> _OnUseCard = null;

    public void OnEnable()
    {
        _highLight.SetActive(false);
    }

    /// <summary>
    /// �J�[�\���������Ă���Ƃ�
    /// </summary>
    public void OnPointEnter()
    {
        if (!UIManager.instance.IsHandAccept) return;

        _highLight.SetActive(true);
    }

    /// <summary>
    /// �J�[�\�����O�ꂽ��
    /// </summary>
    public void OnPointExit()
    {
        if (!UIManager.instance.IsHandAccept) return;

        _highLight.SetActive(false);
    }

    /// <summary>
    /// �h���b�O
    /// </summary>
    public void OnDrag()
    {
        if (!UIManager.instance.IsHandAccept) return;

        Vector3 mousePos = Input.mousePosition;

        // �ړ��ʂ���X���������v�Z�i��ʍ��W��OK�j
        Vector3 move = mousePos - transform.position;

        float tiltFactor = 1; 
        float maxTilt = 20f;

        float tiltX = Mathf.Clamp(move.y * tiltFactor, -maxTilt, maxTilt);
        float tiltY = Mathf.Clamp(-move.x * tiltFactor, -maxTilt, maxTilt);

        // �X����
        transform.localRotation = Quaternion.Euler(tiltX, tiltY, 0);

        // �}�E�X�ʒu�ɒǏ]�i�X�N���[�����W�x�[�X��OK�j
        transform.position = mousePos;
    }


    /// <summary>
    /// �h���b�O�J�n���ꂽ�Ƃ�
    /// </summary>
    public void OnStartDrop()
    {
        if (!UIManager.instance.IsHandAccept) return;

        Transform field = UIManager.instance.GetHandCanvas().transform;
        // �h���b�O�����I�u�W�F�N�g��e����O��
        _handArea = transform.parent;
        transform.SetParent(field);
        // �傫������
        transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
    }

    /// <summary>
    /// �h���b�O�������ꂽ�Ƃ�
    /// </summary>
    public async void OnEndDrop()
    {
        if (!UIManager.instance.IsHandAccept) return;

        // ���̃T�C�Y�ɖ߂�
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        transform.localRotation = Quaternion.identity;

        // �g�p�G���A�Ȃ�
        if (!UIManager.instance.CheckPlayArea(Input.mousePosition))
        {
            // �g�p�G���A�O�Ȃ猳�̈ʒu�ɖ߂�
            transform.SetParent(_handArea);
            return;
        }

        await UseCard();
    }

    private async UniTask UseCard()
    {
        // ���͎�t�I��
        UIManager.instance.EndHandAccept();
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        float duration = 0.5f;
        float elapsed = 0f;

        // �ŏ��ɗ������ɂ���iY��180�x�j
        transform.localRotation = Quaternion.Euler(0, 180f, 0);

        // �\�����ɉ�]���Ȃ��璆���ֈړ�
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime * 2;
            float t = Mathf.Clamp01(elapsed / duration);

            // ����������\������
            float yRot = Mathf.Lerp(180f, 0f, t);
            transform.localRotation = Quaternion.Euler(0, yRot, 0);

            // �ʒu�������֕��
            transform.position = Vector3.Lerp(startPos, endPos, t);

            await UniTask.Yield();
        }
        // �҂�
        await UniTask.Delay(200);

        // �����ăG�t�F�N�g�Đ�
        gameObject.SetActive(false);
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        // �X�N���[����ɍĐ�
        await EffectManager.instance.CreateScreenEffect(0, endPos, Quaternion.identity);
        // �g�p�J�[�h���^�[���ɒʒm
        _OnUseCard(_handIndex);
    }

    /// <summary>
    /// �J�[�h����ݒ�
    /// </summary>
    /// <param name="setID"></param>
    public void SetCard(int setID)
    {
        _ID = setID;
        // �J�[�h���擾
        CardData card = CardManager.GetCard(_ID);
        if (card == null) return;

        _advanceText.text = card.advance.ToString();
        _coinText.text = card.addCoin.ToString();
        Entity_EventData.Param param = EventMasterUtility.GetEventMaster(card.eventID);
        if (param == null)
        {
            _eventText.text = "";
            return;
        }
        int eventTextID = param.textID;
        _eventText.text = eventTextID.ToText();
    }

    /// <summary>
    /// �\���ԍ���ݒ�
    /// </summary>
    /// <param name="index"></param>
    public void SetHandIndex(int index)
    {
        _handIndex = index;
    }

    /// <summary>
    /// �J�[�h���g�����ۂ̃R�[���o�b�N�ݒ�
    /// </summary>
    /// <param name="setCallback"></param>
    public void SetOnUseCard(Action<int> setCallback)
    {
        _OnUseCard = setCallback;
    }
}
