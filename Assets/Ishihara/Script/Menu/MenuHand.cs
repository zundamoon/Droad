using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHand : BaseMenu
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
    /// �g�p�G���A�̃��[�g�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private RectTransform _playArea = null;
    /// <summary>
    /// ���g�p��Ԃ̍��ڂ̃��[�g�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private Transform _unuseRoot = null;

    private List<MenuChoiceList> _useList = null;
    private List<MenuChoiceList> _unuseList = null;

    public RectTransform GetPlayArea()
    {
        return _playArea;
    }
}
