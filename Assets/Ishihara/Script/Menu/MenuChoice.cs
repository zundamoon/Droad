using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CommonModule;

public class MenuChoice : BaseMenu
{
    /// <summary>
    /// ���X�g���ڂ̃I���W�i��
    /// </summary>
    [SerializeField]
    private MenuChoiceList _itemOrigin = null;
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

    private List<MenuChoiceList> _useList = null;
    private List<MenuChoiceList> _unuseList = null;

    private List<int> _choiceCardIDList = null;

    private List<string> _choiceButtonTex = null;

    private System.Action<int> _OnSelect = null;

    public void SetSelectCallback(System.Action<int> onSelect)
    {
        _OnSelect = onSelect;
    }

    public void SetChoiceButtonText(List<string> strings)
    {
        _choiceButtonTex = new List<string>(strings);
    }

    public override async UniTask Initialize()
    {
        await base.Initialize();
        _useList = new List<MenuChoiceList>();
        _unuseList = new List<MenuChoiceList>();
        _choiceCardIDList = new List<int>();
    }

    public void SetChoiceCardID(List<int> setChoiceCardID)
    { 
        _choiceCardIDList = new List<int>(setChoiceCardID); ;
    }

    public async override UniTask Open()
    {
        if (_OnSelect == null) return;

        await base.Open();
        // ���ׂ�
        // �I���������s�K�v��
        for(int i = 0; i < _choiceCardIDList.Count; i += 3)
        {
            // ���X�g���ڂ𐶐�
            MenuChoiceList item = AddListItem();
            for (int j = 0; j < 3; j++)
            {
                // 3�ڂ̑I�����������ꍇ�̓X�L�b�v
                if (i + j >= _choiceCardIDList.Count) break;
                // ���X�g���ڂɃJ�[�h�����Z�b�g
                if(IsEnableIndex(_choiceButtonTex, i + j))
                {
                    item.SetChoiceList(_choiceCardIDList[i + j] ,_choiceButtonTex[i + j]);
                }
                else
                {
                    item.SetChoiceList(_choiceCardIDList[i + j]);
                }
            }
        }
    }

    /// <summary>
    /// ���X�g���ڂ̐���
    /// </summary>
    /// <returns></returns>
    protected MenuChoiceList AddListItem()
    {
        MenuChoiceList addItem;
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
        addItem.Initialized();
        addItem.SetOnselect(_OnSelect);
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
        MenuChoiceList removeItem = _useList[itemIndex];
        removeItem.RemoveList();
        _useList.RemoveAt(itemIndex);
        // ���g�p���X�g�֒ǉ�
        _unuseList.Add(removeItem);
        removeItem.transform.SetParent(_unuseRoot);
    }

    /// <summary>
    /// �S�Ẵ��X�g���ڍ폜
    /// </summary>
    public void RemoveAllItem()
    {
        while (!IsEmpty(_useList)) RemoveListItem(0);
    }
}