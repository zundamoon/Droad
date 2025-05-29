using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using static CommonModule;

public class MenuShop : BaseMenu
{
    /// <summary>
    /// �J�[�h�I��p���j���[
    /// </summary>
    [SerializeField]
    private MenuChoice _menuChoice = null;

    /// <summary>
    /// �u�w���v�{�^��
    /// </summary>
    [SerializeField]
    private Button _buyButton = null;

    /// <summary>
    /// �u���O�v�{�^��
    /// </summary>
    [SerializeField]
    private Button _removalButton = null;

    private List<int> _buyCardIDList = null;
    private List<int> _removalCardIDList = null;

    private UniTaskCompletionSource _isShopActive = null;

    /// <summary>
    /// �񓯊�������
    /// </summary>
    public override async UniTask Initialize()
    {
        await base.Initialize();
        await _menuChoice.Initialize();
        _buyCardIDList = new List<int>();
        _removalCardIDList = new List<int>();
    }

    public async UniTask SetSelectCallback(System.Action<int, bool> onSelect)
    {
        _menuChoice.SetSelectCallback((cardID) =>
        {
            bool isRemove = _buyButton.interactable;
            onSelect(cardID, isRemove);
            if (isRemove)
                RemovalActive();
            else
                BuyActive();
        });

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// �w�����J�[�h��ID��ݒ�
    /// </summary>
    public void SetBuyCardID(List<int> setBuyCardID)
    {
        _buyCardIDList = new List<int>(setBuyCardID);
    }

    /// <summary>
    /// ���O���J�[�h��ID��ݒ�
    /// </summary>
    public void SetRemovalCardID(List<int> setRemovalCardID)
    {
        _removalCardIDList = new List<int>(setRemovalCardID);
    }

    /// <summary>
    /// ���j���[���J��
    /// </summary>
    public override async UniTask Open()
    {
        _isShopActive = new UniTaskCompletionSource();
        await base.Open();
        // �f�t�H���g�ōw�����[�h��\��
        BuyActive(); 

        // �w�������܂ő҂�
        await _isShopActive.Task;
    }

    /// <summary>
    /// ���j���[�����
    /// </summary>
    public async void Close()
    {
        await base.Close(); 
        _isShopActive.TrySetResult();
        await UniTask.Yield();
    }

    /// <summary>
    /// �w�����[�h�ɐ؂�ւ���
    /// </summary>
    public void BuyActive()
    {
        _buyButton.interactable = false;
        _removalButton.interactable = true;

        _menuChoice.RemoveAllItem();
        List<string> ButtonText = new List<string>(_buyCardIDList.Count);
        for (int i = 0; i < _buyCardIDList.Count; i++)
        {
            int CardID = _buyCardIDList[i];
            var Card = CardManager.instance.GetCard(CardID);
            ButtonText.Add(Card.price.ToString());
        }
        _menuChoice.SetChoiceButtonText(ButtonText);
        _menuChoice.SetChoiceCardID(_buyCardIDList);
        // �񓯊������̊J�n
        _menuChoice.Open().Forget(); 
    }

    /// <summary>
    /// ���O���[�h�ɐ؂�ւ���
    /// </summary>
    public void RemovalActive()
    {
        _buyButton.interactable = true;
        _removalButton.interactable = false;

        _menuChoice.RemoveAllItem();
        List<string> ButtonText = new List<string>(_removalCardIDList.Count);
        for (int i = 0; i < _removalCardIDList.Count; i++)
        {
            int CardID = _removalCardIDList[i];
            var Card = CardManager.instance.GetCard(CardID);
            ButtonText.Add(Card.price.ToString());
        }
        _menuChoice.SetChoiceButtonText(ButtonText);
        _menuChoice.SetChoiceCardID(_removalCardIDList);
        _menuChoice.Open().Forget();
    }

    /// <summary>
    /// �V���b�v�̃A�C�e���폜
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="isRemove"></param>
    /// <returns></returns>
    public async UniTask RemoveShopItem(int cardID, bool isRemove)
    {
        if (isRemove)
        {
            _removalCardIDList.Remove(cardID);
            RemovalActive();
        }
        else
        {
            _buyCardIDList.Remove(cardID);
            BuyActive();
        }

        await UniTask.CompletedTask;
    }

    public async UniTask RemoveAllShopItem()
    {
        _menuChoice.RemoveAllItem();
        _buyCardIDList.Clear();
        _removalCardIDList.Clear();
    }
}
