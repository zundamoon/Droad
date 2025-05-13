using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public static Action<int> OnUseCard = null;

    [SerializeField]
    private TextMeshProUGUI _advanceText = null;
    [SerializeField]
    private TextMeshProUGUI _coinText = null;
    [SerializeField]
    private TextMeshProUGUI _eventText = null;
    private int _ID = -1;
    private Transform _handArea;

    public void Start()
    {
        _handArea = transform.parent;
    }

    // �h���b�O
    public void OnDrag()
    {
        if (!UIManager.Instance.IsHandAccept) return;
        
        transform.position = Input.mousePosition;
    }

    // �h���b�O�J�n���ꂽ�Ƃ�
    public void OnStartDrop()
    {
        if (!UIManager.Instance.IsHandAccept) return;

        Transform field = UIManager.Instance.GetHandCanvas().transform;
        // �h���b�O�����I�u�W�F�N�g��e����O��
        transform.SetParent(field);
        // �傫������
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    // �h���b�O�������ꂽ�Ƃ�
    public void OnEndDrop()
    {
        if (!UIManager.Instance.IsHandAccept) return;

        // ���̃T�C�Y�ɖ߂�
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // �g�p�G���A�Ȃ�
        if (!UIManager.Instance.CheckPlayArea(Input.mousePosition))
        {
            // �g�p�G���A�O�Ȃ猳�̈ʒu�ɖ߂�
            transform.SetParent(_handArea);
            return;
        }
        
        // �J�[�h�g�p
        Debug.Log(CardManager.GetCard(_ID).advance + "�i��");
        Debug.Log(CardManager.GetCard(_ID).addCoin + "�R�C���l��");
        //OnUseCard(_ID);
        // ���͎�t�I��
        UIManager.Instance.EndHandAccept();
  
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
}
