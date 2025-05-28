using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    [SerializeField]
    private Button _closeButton = null;

    // ��D�̓��͂���t�����ǂ���
    private bool _IsHandAccept = false;

    private PossessCard possessCard = null;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        await _choice.Initialize();
        await _choice.Close();
        // �`���C�X���j���[�̃{�^���ݒ�
        _choice.SetSelectCallback(async (index) =>
        {
            // ���̃J�[�h�̐��������o��
            await UIManager.instance.OpenCardText(index);
        });
        _closeButton.gameObject.SetActive(false);
    }

    public void SetPosseessCard(ref PossessCard setPossessCard)
    {
        possessCard = setPossessCard;
    }

    /// <summary>
    /// �f�b�L�{�^���������ꂽ�Ƃ�
    /// </summary>
    public async void OnShowDeck()
    {
        if (possessCard == null) return;
        // ��D�̓��͏I��
        _IsHandAccept = UIManager.instance.IsHandAccept;
        UIManager.instance.EndHandAccept();
        // �{�^�����\��
        _deckDetail.gameObject.SetActive(false);
        _discardAreaDetail.gameObject.SetActive(false);
        _choice.RemoveAllItem();
        // �`���C�X���j���[�ɐݒ�
        List<int> deck = possessCard.deckCardIDList;
        _choice.SetChoiceCardID(deck);
        List<string> deckText = new List<string>(deck.Count);
        for (int i = 0; i < deck.Count; i++)
        {
            deckText.Add("�ڍ�");
        }
        _choice.SetChoiceButtonText(deckText);

        _closeButton.gameObject.SetActive(true);
        await _choice.Open();
    }

    /// <summary>
    /// �̂ĎD�{�^���������ꂽ�Ƃ�
    /// </summary>
    public async void OnShowDiscard()
    {
        if (possessCard == null) return;

        // ��D�̓��͏I��
        _IsHandAccept = UIManager.instance.IsHandAccept;
        UIManager.instance.EndHandAccept();
        // �{�^�����\��
        _deckDetail.gameObject.SetActive(false);
        _discardAreaDetail.gameObject.SetActive(false);
        _choice.RemoveAllItem();

        // �`���C�X���j���[�ɐݒ�
        List<int> deck = possessCard.discardCardIDList;
        _choice.SetChoiceCardID(deck);
        List<string> deckText = new List<string>(deck.Count);
        for (int i = 0; i < deck.Count; i++)
        {
            deckText.Add("�ڍ�");
        }
        _choice.SetChoiceButtonText(deckText);
        _closeButton.gameObject.SetActive(true);
        await _choice.Open();
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
        _closeButton.gameObject.SetActive(false);

        await UIManager.instance.CloseCardText();
        // �E�B���h�E�����
        await _choice.Close();
    }
}
