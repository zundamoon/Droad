using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : SystemObject
{
    public static UIManager instance { get; private set; } = null;

    [SerializeField]
    private MenuChoice _menuChoice = null;

    [SerializeField]
    private MenuHand _menuHand = null;

    [SerializeField]
    private MessageUI _messageUI = null;

    [SerializeField]
    private MenuStatus _menuStatus = null;

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

        await _menuHand.Initialize();
        await _menuChoice.Initialize();
        await _menuStatus.Initialize();
        _menuStatus.SetCharacter(CharacterManager.instance.GetAllCharacter());

        await _menuHand.Close();
        await _menuChoice.Close();
        await _messageUI.Inactive();
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
        _menuHand.SetTurnPlayerCard(setPossessCard);
        await _menuHand.Open();
    }

    /// <summary>
    /// ��D�G���A�����
    /// </summary>
    /// <returns></returns>
    public async UniTask CloseHandArea()
    {
        await _menuHand.Close();
    }

    /// <summary>
    /// �I�����G���A���J��
    /// </summary>
    /// <returns></returns>
    public async UniTask OpenChoiceArea(List<int> choiceCardIDList)
    {
        _menuChoice.SetChoiceCardID(choiceCardIDList);
        await _menuChoice.Open();
    }

    /// <summary>
    /// �I�����G���A�����
    /// </summary>
    /// <returns></returns>
    public async UniTask CloseChoiceArea()
    {
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
}
