using System.Collections.Generic;
using UnityEngine;
using static CommonModule;

public class MenuChoiceList : MonoBehaviour
{
    /// <summary>
    /// �J�[�h�I�����̃v���n�u
    /// </summary>
    [SerializeField]
    private MenuChoiceCard _choiceCardOriginal = null;

    /// <summary>
    /// �\�����̃J�[�h����ׂ�e�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private Transform _contentRoot = null;

    /// <summary>
    /// ��\����Ԃ̃J�[�h���i�[����e�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private Transform _unuseRoot = null;

    private const int _maxCardItem = 3;

    private List<MenuChoiceCard> _useCardList = null;
    private List<MenuChoiceCard> _unuseCardList = null;

    private System.Action<int> _OnSelect;

    public void SetOnselect(System.Action<int> onSelect)
    {
        _OnSelect = onSelect;
    }

    /// <summary>
    /// �����������i�J�[�h�I�u�W�F�N�g�����O�ɐ������Ė��g�p���X�g�Ɋi�[�j
    /// </summary>
    public void Initialized()
    {
        _useCardList = new List<MenuChoiceCard>(_maxCardItem);
        _unuseCardList = new List<MenuChoiceCard>(_maxCardItem);

        for (int i = 0; i < _maxCardItem; i++)
        {
            // �v���n�u����J�[�h�𐶐������g�p���X�g��
            var item = Instantiate(_choiceCardOriginal, _unuseRoot);
            _unuseCardList.Add(item);
        }
    }

    /// <summary>
    /// �J�[�hID�ɑΉ�����J�[�h��ǉ�
    /// </summary>
    public void SetChoiceList(int choiceCardID)
    {
        // ���g�p����擾�܂��͐V�K����
        MenuChoiceCard addItem = AddListItem();

        // �J�[�h���e�ݒ�
        addItem.SetCard(choiceCardID);

        // �������̃A�N�V�����ݒ�
        addItem.SetButtonAction(() =>
        {
            _OnSelect(choiceCardID);
        });
    }

    /// <summary>
    /// �J�[�hID�ɑΉ�����J�[�h��ǉ�
    /// </summary>
    public void SetChoiceList(int choiceCardID, string buttonText)
    {
        // ���g�p����擾�܂��͐V�K����
        MenuChoiceCard addItem = AddListItem();

        // �J�[�h���e�ݒ�
        addItem.SetCard(choiceCardID);

        addItem.SetButtonText(buttonText);

        // �������̃A�N�V�����ݒ�
        addItem.SetButtonAction(() =>
        {
            _OnSelect(choiceCardID);
        });
    }

    /// <summary>
    /// �J�[�h���ڂ𖢎g�p���X�g����擾�A�܂��͐���
    /// </summary>
    protected MenuChoiceCard AddListItem()
    {
        MenuChoiceCard addItem;
        if (IsEmpty(_unuseCardList))
        {
            // ���g�p���Ȃ��Ȃ�V��������
            addItem = Instantiate(_choiceCardOriginal, _contentRoot);
        }
        else
        {
            // ���g�p����ė��p
            addItem = _unuseCardList[0];
            _unuseCardList.RemoveAt(0);
            addItem.transform.SetParent(_contentRoot, false);
        }

        _useCardList.Add(addItem);
        return addItem;
    }

    /// <summary>
    /// �g�p���̃J�[�h�����ׂĖ��g�p�ɖ߂�
    /// </summary>
    public void RemoveList()
    {
        foreach (var item in _useCardList)
        {
            item.transform.SetParent(_unuseRoot, false);
            item.InitButtonText();
            _unuseCardList.Add(item);
        }
        _useCardList.Clear();
    }
}
