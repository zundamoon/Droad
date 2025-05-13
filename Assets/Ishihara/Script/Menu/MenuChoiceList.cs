using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static CommonModule;

public class MenuChoiceList : MonoBehaviour
{
    // �ێ�����J�[�h�I�����̃I���W�i��
    [SerializeField]
    private MenuChoiceCard _choiceCardOriginal = null;

    /// <summary>
    /// ���ڂ���ׂ郋�[�g�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private Transform _contentRoot = null;
    /// <summary>
    /// ���g�p��Ԃ̍��ڂ̃��[�g�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private Transform _unuseRoot = null;

    private const int _maxCardItem = 3;

    private List<MenuChoiceCard> _useCardList = null;
    private List<MenuChoiceCard> _unuseCardList = null;

    public void Initialized()
    {
        _unuseCardList = new List<MenuChoiceCard>(_maxCardItem);
        _useCardList = new List<MenuChoiceCard>(_maxCardItem);
        for (int i = 0; i < _maxCardItem; i++)
        {
            // ���g�p���X�g�ɒǉ�
            var item = Instantiate(_choiceCardOriginal, _unuseRoot);
            _unuseCardList.Add(item);
        }
    }

    public void SetChoiceList(int choicecardID)
    {
        // ���g�p���X�g����擾
        MenuChoiceCard addItem = AddListItem();
        addItem.SetCard(choicecardID);
        _useCardList.Add(addItem);
        // �{�^���A�N�V����
        addItem.SetButtonAction(() =>
        {
            // �I������I���������̏���
            Debug.Log($"�I����{choicecardID}���I������܂���");
        });
    }

    /// <summary>
    /// ���X�g���ڂ̐���
    /// </summary>
    /// <returns></returns>
    protected MenuChoiceCard AddListItem()
    {
        MenuChoiceCard addItem;
        if (IsEmpty(_unuseCardList))
        {
            // ���g�p���X�g����Ȃ̂Ő���
            addItem = Instantiate(_choiceCardOriginal, _contentRoot);
        }
        else
        {
            // ���g�p���X�g����擾
            addItem = _unuseCardList[0];
            _unuseCardList.RemoveAt(0);
            addItem.transform.SetParent(_contentRoot);
        }
        _useCardList.Add(addItem);
        return addItem;
    }

    public void RemoveList()
    {
        for(int i = 0; i < _useCardList.Count; i++)
        {
            // ���g�p�G���A�Ɉړ�
            var item = _useCardList.FirstOrDefault();
            if (item == null) continue;
            item.transform.SetParent(_unuseRoot);
            _unuseCardList.Add(item);
            _useCardList.Remove(item);
        }
    }
}
