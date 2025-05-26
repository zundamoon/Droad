using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuDetail : BaseMenu
{
    [SerializeField]
    private MenuChoice _choice = null;

    [SerializeField]
    private Button _deckDetail = null;

    [SerializeField]
    private Button _discardAreaDetail = null;

    // ��D�̓��͂���t�����ǂ���
    private bool _IsHandAccept = false;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        // �`���C�X���j���[�̃{�^���ݒ�
        _choice.SetSelectCallback(async (index) =>
        {
            // ���̃J�[�h�̐��������o��
            await UIManager.instance.OpenCardText(index);
        });
    }

    /// <summary>
    /// �f�b�L�{�^���������ꂽ�Ƃ�
    /// </summary>
    public void OnShowDeck()
    {
        // ��D�̓��͏I��
        _IsHandAccept = UIManager.instance.IsHandAccept;
        UIManager.instance.EndHandAccept();
        // �{�^�����\��
        _deckDetail.gameObject.SetActive(false);
        _discardAreaDetail.gameObject.SetActive(false);

        // �f�b�L��\��

        // �ǂ�����^�[���v���C���[���擾����̂�
        // CardText���ǂ̃^�C�~���O�ŕ���̂�

        // �^�[���v���C���[���擾
        // �f�b�L�擾
        // �`���C�X���j���[�ɐݒ�
        // 

    }

    /// <summary>
    /// �̂ĎD�{�^���������ꂽ�Ƃ�
    /// </summary>
    public void OnShowDiscard()
    {
        // ��D�̓��͏I��
        _IsHandAccept = UIManager.instance.IsHandAccept;
        UIManager.instance.EndHandAccept();
        // �{�^�����\��
        _deckDetail.gameObject.SetActive(false);
        _discardAreaDetail.gameObject.SetActive(false);

        // �̂ĎD��\��

    }

    /// <summary>
    /// �ڍ׉�ʂ̏I��
    /// </summary>
    public async void OnEndDetail()
    {
        // ��D�̓��͎�t���ĊJ
        if(_IsHandAccept) 
            UIManager.instance.StartHandAccept();

        // �{�^���̕\���ĊJ
        _deckDetail.gameObject.SetActive(true);
        _discardAreaDetail.gameObject.SetActive(true);

        // �E�B���h�E�����
        await Close();
    }
}
