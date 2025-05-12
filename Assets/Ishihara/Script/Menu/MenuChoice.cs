using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CommonModule;

public class MenuList : BaseMenu
{
    /// <summary>
    /// ���X�g���ڂ̃I���W�i��
    /// </summary>
    [SerializeField]
    private MenuChoiceItem _itemOrigin = null;
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

    private int _currentIndex = -1;

    private List<MenuChoiceItem> _useList = null;
    private List<MenuChoiceItem> _unuseList = null;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        _useList = new List<MenuChoiceItem>();
        _unuseList = new List<MenuChoiceItem>();
    }

    /// <summary>
    /// ���X�g���ڂ̐���
    /// </summary>
    /// <returns></returns>
    protected MenuChoiceItem AddListItem()
    {
        MenuChoiceItem addItem;
        if (IsEmpty(_unuseList))
        {
            // ���g�p���X�g����Ȃ̂Ő���
            addItem = Instantiate(_itemOrigin, _contentRoot);
        }
        else
        {
            // ���g�p���X�g����擾
            addItem = _unuseList[0];
            _unuseList.RemoveAt(0);
            addItem.transform.SetParent(_contentRoot);
        }
        addItem.Deselect();
        _useList.Add(addItem);
        return addItem;
    }

    /// <summary>
    /// �C���f�N�X�w��̃��X�g���ڍ폜
    /// </summary>
    /// <param name="itemIndex"></param>
    protected void RemoveListItem(int itemIndex)
    {
        if (!IsEnableIndex(_useList, itemIndex)) return;
        // �g�p���X�g�����菜��
        MenuChoiceItem removeItem = _useList[itemIndex];
        _useList.RemoveAt(itemIndex);
        // ���g�p���X�g�֒ǉ�
        _unuseList.Add(removeItem);
        removeItem.transform.SetParent(_unuseRoot);
        removeItem.Deselect();
    }

    /// <summary>
    /// �S�Ẵ��X�g���ڍ폜
    /// </summary>
    protected void RemoveAllItem()
    {
        while (!IsEmpty(_useList)) RemoveListItem(0);

    }
}