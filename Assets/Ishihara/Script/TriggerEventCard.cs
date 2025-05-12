using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEventCard : MonoBehaviour
{
    private Transform _handArea;

    public void Start()
    {
        _handArea = transform.parent;
    }

    // �h���b�O
    public void OnDrag()
    {
        if (!UIManager.Instance.IsHandAccept) return;
        
        this.transform.position = Input.mousePosition;
    }

    // �h���b�O�J�n���ꂽ�Ƃ�
    public void OnStartDrop()
    {
        if (!UIManager.Instance.IsHandAccept) return;

        Transform field = UIManager.Instance.GetHandCanvas().transform;
        // �h���b�O�����I�u�W�F�N�g��e����O��
        this.transform.SetParent(field);
    }

    // �h���b�O�������ꂽ�Ƃ�
    public void OnEndDrop()
    {
        if (!UIManager.Instance.IsHandAccept) return;

        // �g�p�G���A�Ȃ�
        if (!UIManager.Instance.CheckPlayArea(Input.mousePosition))
        {
            // �g�p�G���A�O�Ȃ猳�̈ʒu�ɖ߂�
            this.transform.SetParent(_handArea);
            return;
        }
        
        // �J�[�h�g�p
        Debug.Log("�J�[�h�g�p");
        // ���͎�t�I��
        UIManager.Instance.EndHandAccept();
  
        Destroy(this.gameObject);
    }
}
