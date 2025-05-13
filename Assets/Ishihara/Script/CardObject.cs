using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    private static Action<int> _OnUseCard = null;

    [SerializeField]
    private TextMeshProUGUI _advanceText = null;
    [SerializeField]
    private TextMeshProUGUI _coinText = null;
    [SerializeField]
    private TextMeshProUGUI _eventText = null;
    private int _ID = -1;
    private Transform _handArea;

    // �h���b�O
    public void OnDrag()
    {
        if (!UIManager.instance.IsHandAccept) return;
        
        transform.position = Input.mousePosition;
    }

    // �h���b�O�J�n���ꂽ�Ƃ�
    public void OnStartDrop()
    {
        if (!UIManager.instance.IsHandAccept) return;

        Transform field = UIManager.instance.GetHandCanvas().transform;
        // �h���b�O�����I�u�W�F�N�g��e����O��
        _handArea = transform.parent;
        transform.SetParent(field);
        // �傫������
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    // �h���b�O�������ꂽ�Ƃ�
    public void OnEndDrop()
    {
        if (!UIManager.instance.IsHandAccept) return;

        // ���̃T�C�Y�ɖ߂�
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // �g�p�G���A�Ȃ�
        if (!UIManager.instance.CheckPlayArea(Input.mousePosition))
        {
            // �g�p�G���A�O�Ȃ猳�̈ʒu�ɖ߂�
            transform.SetParent(_handArea);
            return;
        }
        
        // �g�p�J�[�h���^�[���ɒʒm
        _OnUseCard(_ID);
        // ���͎�t�I��
        UIManager.instance.EndHandAccept();
  
        Destroy(gameObject);
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
    /// �J�[�h���g�����ۂ̃R�[���o�b�N�ݒ�
    /// </summary>
    /// <param name="setCallback"></param>
    public static void SetOnUseCard(Action<int> setCallback)
    {
        _OnUseCard = setCallback;
    }
}
