using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

using static CommonModule;

public class MenuStatus : BaseMenu
{
    /// <summary>
    /// ���X�g���ڂ̃I���W�i��
    /// </summary>
    [SerializeField]
    private MenuStatusChara _charaStatusOrigin = null;
    [SerializeField]
    private MenuStatusEvent _eventStatusOrigin = null;
    
    /// <summary>
    /// ���ׂ�w�i
    /// </summary>
    [SerializeField]
    private RectTransform _BGRect = null;
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

    // ���ԃ��X�g
    private List<MenuStatusItem> _statusOrderList = null;

    private List<MenuStatusChara> _useCharaList = null;
    private List<MenuStatusChara> _unuseCharaList = null;
    private List<MenuStatusEvent> _useEventList = null;
    private List<MenuStatusEvent> _unuseEventList = null;

    private const int _STATUS_MAX = 5;
    private const int _EVENT_MAX = 3;

    private List<Character> _characters = null;

    public void SetCharacter(List<Character> setCharacters)
    {
        _characters = setCharacters;
    }

    public async UniTask ScrollStatus()
    {
        if (!IsEnableIndex(_statusOrderList, 0)) return;

        _statusOrderList.RemoveAt(0);

        if (!IsEnableIndex(_statusOrderList, 0)) return;

        List<UniTask> tasks = new List<UniTask>();

        Vector3 pos = _BGRect.position;

        for (int i = 0; i < _statusOrderList.Count; i++)
        {
            var status = _statusOrderList[i];

            UniTask task = status.Move(pos);
            tasks.Add(task);

            pos.y += status.GetHeight();
            // �X�y�[�X
            pos.y += 20;
        }

       await UniTask.WhenAll(tasks);
    }

    public void AddStatus()
    {
        var status = AddCharaStatusItem();
        _statusOrderList.Add(status);
    }
    
    public override async UniTask Initialize()
    {
        await base.Initialize();
        _useCharaList = new List<MenuStatusChara>();
        _unuseCharaList = new List<MenuStatusChara>();
        for (int i = 0; i < _STATUS_MAX; i++)
        {
            var status = Instantiate(_charaStatusOrigin, _unuseRoot);
            _unuseCharaList.Add(status);
        }
        //_useEventList = new List<MenuStatusEvent>();
        //_unuseEventList = new List<MenuStatusEvent>();
        //for (int i = 0; i < _EVENT_MAX; i++)
        //{
        //    var status = Instantiate(_eventStatusOrigin, _unuseRoot);
        //    _unuseEventList.Add(status);
        //}
        //_statusOrderList = new List<MenuStatusItem>();
        _characters = new List<Character>();
    }

    /// <summary>
    /// ���X�g���ڂ̐���
    /// </summary>
    /// <returns></returns>
    private MenuStatusChara AddCharaStatusItem()
    {
        MenuStatusChara addItem;
        if (IsEmpty(_unuseCharaList))
        {
            // ���g�p���X�g����Ȃ̂Ő���
            addItem = Instantiate(_charaStatusOrigin, _contentRoot);
        }
        else
        {
            // ���g�p���X�g����擾
            addItem = _unuseCharaList[0];
            _unuseCharaList.RemoveAt(0);
            addItem.transform.SetParent(_contentRoot);
        }
        //addItem.Initialized();
        _useCharaList.Add(addItem);
        return addItem;
    }

    /// <summary>
    /// ���X�g���ڂ̐���
    /// </summary>
    /// <returns></returns>
    private MenuStatusEvent AddEventStatusItem()
    {
        MenuStatusEvent addItem;
        if (IsEmpty(_unuseEventList))
        {
            // ���g�p���X�g����Ȃ̂Ő���
            addItem = Instantiate(_eventStatusOrigin, _contentRoot);
        }
        else
        {
            // ���g�p���X�g����擾
            addItem = _unuseEventList[0];
            _unuseEventList.RemoveAt(0);
            addItem.transform.SetParent(_contentRoot);
        }
        //addItem.Initialized();
        _useEventList.Add(addItem);
        return addItem;
    }

    /// <summary>
    /// �C���f�N�X�w��̃��X�g���ڍ폜
    /// </summary>
    /// <param name="itemIndex"></param>
    private void RemoveCharaStatus(int itemIndex)
    {
        if (!IsEnableIndex(_useCharaList, itemIndex)) return;
        // �g�p���X�g�����菜��
        MenuStatusChara removeItem = _useCharaList[itemIndex];
        _useCharaList.RemoveAt(itemIndex);
        // ���g�p���X�g�֒ǉ�
        _unuseCharaList.Add(removeItem);
        removeItem.transform.SetParent(_unuseRoot);
    }
    /// <summary>
    /// �C���f�N�X�w��̃��X�g���ڍ폜
    /// </summary>
    /// <param name="itemIndex"></param>
    private void RemoveEventStatus(int itemIndex)
    {
        if (!IsEnableIndex(_useEventList, itemIndex)) return;
        // �g�p���X�g�����菜��
        MenuStatusEvent removeItem = _useEventList[itemIndex];
        _useEventList.RemoveAt(itemIndex);
        // ���g�p���X�g�֒ǉ�
        _unuseEventList.Add(removeItem);
        removeItem.transform.SetParent(_unuseRoot);
    }

    /// <summary>
    /// �S�Ẵ��X�g���ڍ폜
    /// </summary>
    public void RemoveAllStatus()
    {
        while (!IsEmpty(_useCharaList)) RemoveCharaStatus(0);
        while (!IsEmpty(_useEventList)) RemoveEventStatus(0);
    }

    public void ShowSelfData(int index, GUIStyle style)
    {
        GUILayout.Label( (index + 1) + "P", style);
        GUILayout.Label("coin" + _characters[index].coins, style);
        GUILayout.Label("star" + _characters[index].stars, style);
    }
}
