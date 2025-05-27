using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : SystemObject
{
    public static UIManager instance { get; private set; } = null;

    // �I����
    [SerializeField]
    private MenuChoice _menuChoice = null;
    // ��DUI
    [SerializeField]
    private MenuHand _menuHand = null;
    // ���b�Z�[�WUI
    [SerializeField]
    private MessageUI _messageUI = null;
    // ����UI
    [SerializeField]
    private MenuStatus _menuStatus = null;
    // �V���b�v
    [SerializeField]
    private MenuShop _menuShop = null;
    // �J�[�h�e�L�X�g
    [SerializeField]
    private MenuCardText _menuCardText = null;

    private UniTaskCompletionSource _handUniTaskCompletionSource = null;
    private UniTaskCompletionSource _choiceUniTaskCompletionSource = null;

    private const float _DEFAULT_DISPLAY_TIME = 0.75f;

    public bool IsHandAccept { get; private set; } = false;

    public async override UniTask Initialize()
    {
        await base.Initialize();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // ���j���[�𐶐�
        _menuHand = Instantiate(_menuHand);
        _menuChoice = Instantiate(_menuChoice);
        _messageUI = Instantiate(_messageUI);
        _menuStatus = Instantiate(_menuStatus);
        _menuShop = Instantiate(_menuShop);
        _menuCardText = Instantiate(_menuCardText);

        await _menuHand.Initialize();
        await _menuChoice.Initialize();
        await _menuStatus.Initialize();
        await _menuShop.Initialize();
        await _menuCardText.Initialize();
        _menuStatus.SetCharacter(CharacterManager.instance.GetAllCharacter());

        await _menuHand.Close();
        await _menuChoice.Close();
        await _messageUI.Inactive();
        await _menuCardText.Close();
        _menuShop.Open();
        _menuShop.Close();
    }

    public GameObject GetHandCanvas()
    {
        return _menuHand.GetCanvas();
    }

    /// <summary>
    /// �g�p�G���A���ǂ���
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckPlayArea(Vector2 pos)
    {
        RectTransform playArea = _menuHand.GetPlayArea();
        if (playArea == null)
        {
            return false;
        }

        Vector2 localPos;
        // �X�N���[�����W�����[�J�����W�ɕϊ�
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(playArea, pos, null, out localPos))
        {
            return false;
        }

        // �͈͓����ǂ�������
        return playArea.rect.Contains(localPos);
    }

    /// <summary>
    /// ��D�G���A���J��
    /// </summary>
    /// <returns></returns>
    public async UniTask OpenHandArea(PossessCard setPossessCard)
    {
        _handUniTaskCompletionSource = new UniTaskCompletionSource();

        _menuHand.SetTurnPlayerCard(setPossessCard);
        await _menuHand.Open();
        StartHandAccept();

        await _handUniTaskCompletionSource.Task;
    }

    /// <summary>
    /// ��D�G���A�����
    /// </summary>
    /// <returns></returns>
    public async UniTask CloseHandArea()
    {
        _menuHand.RemoveAllItem();
        await _menuHand.Close();
    }

    /// <summary>
    /// �I�����G���A���J��
    /// </summary>
    /// <returns></returns>
    public async UniTask OpenChoiceArea(List<int> choiceCardIDList)
    {
        _choiceUniTaskCompletionSource = new UniTaskCompletionSource();

        _menuChoice.SetChoiceCardID(choiceCardIDList);
        await _menuChoice.Open();

        await _choiceUniTaskCompletionSource.Task;
    }

    public async UniTask SetChoiceCallback(System.Action<int> action)
    {
        _menuChoice.SetSelectCallback(async (index) =>
        {
            action(index);
            _choiceUniTaskCompletionSource.TrySetResult();
            await CloseChoiceArea();
        });
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// �I�����G���A�����
    /// </summary>
    /// <returns></returns>
    public async UniTask CloseChoiceArea()
    {
        _menuChoice.RemoveAllItem();
        await _menuChoice.Close();
    }

    /// <summary>
    /// ��D���͎�t�J�n
    /// </summary>
    public void StartHandAccept()
    {
        IsHandAccept = true;
    }

    /// <summary>
    /// ��D���͎�t�I��
    /// </summary>
    public void EndHandAccept()
    {
        IsHandAccept = false;
    }

    /// <summary>
    /// ���b�Z�[�WUI��\������
    /// </summary>
    /// <param name="text"></param>
    /// <param name="displayTime"></param>
    /// <returns></returns>
    public async UniTask RunMessage(string text, float displayTime = _DEFAULT_DISPLAY_TIME)
    {
        await _messageUI.RunMessage(text, displayTime);
    }

    /// <summary>
    /// �X�e�[�^�X�G���A�ɒǉ�����
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public async UniTask AddStatus(int CharacterID)
    {
        Character character = CharacterManager.instance.GetCharacter(CharacterID);
        await _menuStatus.AddStatus(character);
    }

    /// <summary>
    /// �X�e�[�^�X�G���A�ɒǉ�����
    /// </summary>
    /// <returns></returns>
    public async UniTask AddStatus(string str)
    {
        await _menuStatus.AddStatus(str);
    }

    /// <summary>
    /// �X�e�[�^�X�G���A�ɒǉ�����
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public async UniTask AddStatus(List<int> CharacterID)
    {
        for (int i = 0; i < CharacterID.Count; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(CharacterID[i]);
            await _menuStatus.AddStatus(character);
        }
    }

    /// <summary>
    /// �X�e�[�^�X�G���A���X�N���[������
    /// </summary>
    /// <returns></returns>
    public async UniTask ScrollStatus()
    {
        await _menuStatus.ScrollStatus();
    }

    /// <summary>
    /// �擪�̃X�e�[�^�X��傫������
    /// </summary>
    /// <returns></returns>
    public async UniTask ReSizeTop()
    {
        await _menuStatus.ReSizeTop();
    }

    /// <summary>
    /// �X�e�[�^�X�G���A���N���A����
    /// </summary>
    /// <returns></returns>
    public async UniTask ClearStatus()
    {
        await _menuStatus.RemoveAllStatus();
    }

    /// <summary>
    /// �S�ẴX�e�[�^�X���X�N���[������
    /// </summary>
    /// <returns></returns>
    public async UniTask ScrollAllStatus()
    {
        await _menuStatus.ScrollAllStatus();
    }

    /// <summary>
    /// �V���b�v���J��
    /// </summary>
    /// <returns></returns>
    public async UniTask OpenShop()
    {
        await _menuShop.Open();
    }

    /// <summary>
    /// �V���b�v�����
    /// </summary>
    /// <returns></returns>
    public async UniTask CloseShop()
    {
        _menuShop.Close();
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// �w���A�C�e���̐ݒ�
    /// </summary>
    /// <param name="itemIDList"></param>
    /// <returns></returns>
    public async UniTask SetBuyItem(List<int> itemIDList)
    {
        _menuShop.SetBuyCardID(itemIDList);
        await UniTask.CompletedTask;

    }

    /// <summary>
    /// ���O�A�C�e���̐ݒ�
    /// </summary>
    /// <param name="itemIDList"></param>
    /// <returns></returns>
    public async UniTask SetRemovaItem(List<int> itemIDList)
    {
        _menuShop.SetRemovalCardID(itemIDList);
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// �w�������R�[���o�b�N�̐ݒ�
    /// </summary>
    /// <param name="onSelect"></param>
    /// <returns></returns>
    public async UniTask SetSelectCallback(System.Action<int, bool> onSelect)
    {
        await _menuShop.SetSelectCallback(onSelect);
    }

    /// <summary>
    /// �V���b�v�̃A�C�e���폜
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="isRemove"></param>
    /// <returns></returns>
    public async UniTask RemoveShopItem(int cardID, bool isRemove = false)
    {
        await _menuShop.RemoveShopItem(cardID, isRemove);
    }

    /// <summary>
    /// �V���b�v�̑S�A�C�e���폜
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="isRemove"></param>
    /// <returns></returns>
    public async UniTask RemoveAllShopItem()
    {
        await _menuShop.RemoveAllShopItem();
    }

    /// <summary>
    /// �J�[�h�g�p�̐ݒ�
    /// </summary>
    /// <returns></returns>
    public async UniTask SetOnUseCard(System.Action<int> action)
    {
        _menuHand.SetOnUseCard(async (index) =>
        {
            action(index);
            _handUniTaskCompletionSource.TrySetResult();
            await CloseHandArea();
        });
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// �J�[�h�e�L�X�g���J��
    /// </summary>
    /// <param name="cardID"></param>
    /// <returns></returns>
    public async UniTask OpenCardText(int cardID)
    {
        await _menuCardText.SetText(cardID);
        await _menuCardText.Open();
    }

    /// <summary>
    /// �J�[�h�e�L�X�g�����
    /// </summary>
    /// <returns></returns>
    public async UniTask CloseCardText()
    {
        await _menuCardText.Close();
    }

    /// <summary>
    /// ��D�ɃJ�[�h���̂Ă�
    /// </summary>
    /// <param name="handIndex"></param>
    /// <returns></returns>
    public async UniTask HandDiscard(int handIndex)
    {
        // �J�[�h����D���珜�O
        await _menuHand.DiscardHandCard(handIndex);
    }

    /// <summary>
    /// ��D�ɃJ�[�h��ǉ�����
    /// </summary>
    /// <param name="cardID"></param>
    /// <returns></returns>
    public async UniTask HandDraw(int cardID)
    {
        // �J�[�h����D�ɒǉ�
        await _menuHand.AddHandCard(cardID);
    }

    /// <summary>
    /// �L�����̃X�e�[�^�X���X�V����
    /// </summary>
    /// <param name="chara"></param>
    /// <returns></returns>
    public async UniTask UpdateStatus(Character chara)
    {
        await _menuStatus.UpdateStatus(chara);
    }
}
