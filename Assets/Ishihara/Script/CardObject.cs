using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardObject : MonoBehaviour
{
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
    }

    // �h���b�O�������ꂽ�Ƃ�
    public void OnEndDrop()
    {
        if (!UIManager.Instance.IsHandAccept) return;

        // �g�p�G���A�Ȃ�
        if (!UIManager.Instance.CheckPlayArea(Input.mousePosition))
        {
            // �g�p�G���A�O�Ȃ猳�̈ʒu�ɖ߂�
            transform.SetParent(_handArea);
            return;
        }
        
        // �J�[�h�g�p
        Debug.Log("�J�[�h�g�p");
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
        int eventTextID = EventMasterUtility.GetEventMaster(card.eventID).textID;
        _eventText.text = eventTextID.ToText();
    }
}
