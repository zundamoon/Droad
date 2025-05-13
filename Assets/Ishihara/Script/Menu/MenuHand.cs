using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

using static CommonModule;

public class MenuHand : BaseMenu
{
    /// <summary>
    /// ���X�g���ڂ̃I���W�i��
    /// </summary>
    [SerializeField]
    private CardObject _itemOrigin = null;
    /// <summary>
    /// ���ڂ���ׂ郋�[�g�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private Transform _contentRoot = null;
    /// <summary>
    /// �g�p�G���A�̃��[�g�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private RectTransform _playArea = null;
    /// <summary>
    /// ���g�p��Ԃ̍��ڂ̃��[�g�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private Transform _unuseRoot = null;

    private List<CardObject> _useList = null;
    private List<CardObject> _unuseList = null;

    private PossessCard _possessCard = null;
    private readonly int _MAX_HAND_CARD = 4;

    public void SetTurnPlayerCard(PossessCard setPossessCard)
    {
        _possessCard = setPossessCard;
    }

    public async override UniTask Initialize()
    {
        await base.Initialize();
        _useList = new List<CardObject>();
        _unuseList = new List<CardObject>();
        for (int i = 0; i < _MAX_HAND_CARD; i++)
        {
            var item = Instantiate(_itemOrigin, _unuseRoot);
            _unuseList.Add(item);
        }
    }

    public async override UniTask Open()
    {
        RemoveAllItem();

        await base.Open();
        // ���ׂ�
        // ��D�����擾
        int handCount = _possessCard.handCardIDList.Count;
        // ���ׂ�
        for (int i = 0; i < handCount; i++)
        {
            // �g�p�G���A�Ɉړ�
            var item = AddListItem();
            // �J�[�h���X�V
            item.SetCard(_possessCard.handCardIDList[i]);
        }
    }

    protected CardObject AddListItem()
    {
        CardObject addItem;
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
        CardObject removeItem = _useList[itemIndex];
        _useList.RemoveAt(itemIndex);
        // ���g�p���X�g�֒ǉ�
        _unuseList.Add(removeItem);
        removeItem.transform.SetParent(_unuseRoot);
    }

    /// <summary>
    /// �S�Ẵ��X�g���ڍ폜
    /// </summary>
    protected void RemoveAllItem()
    {
        while (!IsEmpty(_useList)) RemoveListItem(0);

    }

    public RectTransform GetPlayArea()
    {
        return _playArea;
    }
}
