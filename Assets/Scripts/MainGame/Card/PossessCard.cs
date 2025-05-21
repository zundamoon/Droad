using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PossessCard
{
    /// <summary>
    /// �����J�[�h���X�g
    /// </summary>
    public List<int> possessCardIDList { get; private set; } = null;
    /// <summary>
    /// �f�b�L�J�[�h���X�g
    /// </summary>
    public List<int> deckCardIDList { get; private set; } = null;
    /// <summary>
    /// ��D�J�[�h���X�g
    /// </summary>
    public List<int> handCardIDList { get; private set; } = null;
    /// <summary>
    /// �̂ăJ�[�h���X�g
    /// </summary>
    public List<int> discardCardIDList { get; private set; } = null;
    public List<int> starCardIDList { get; private set; } = null;

    private Action<int> _AddStarCallback = null;
    private Func<int, int> _LoseStarCallback = null;

    private const int _DEFAULT_DECK_MAX = 12;
    private const int _HAND_MAX = 4;
    private const int _GET_STAR_TEXT_ID = 106;
    private const int _RESHUFFLE_TEXT_ID = 107;

    public void Init()
    {
        possessCardIDList = new List<int>(_DEFAULT_DECK_MAX);
        deckCardIDList = new List<int>(_DEFAULT_DECK_MAX);
        handCardIDList = new List<int>(_DEFAULT_DECK_MAX);
        discardCardIDList = new List<int>(_DEFAULT_DECK_MAX);

        // ������D�ݒ�
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                possessCardIDList.Add(i);
                deckCardIDList.Add(i);
            }
        }
        ShuffleDeck();
    }

    /// <summary>
    /// �f�b�L���V���b�t������
    /// </summary>
    public void ShuffleDeck()
    {
        int deckCount = deckCardIDList.Count;
        for (int i = deckCount - 1; i > 0; i--)
        {
            // �����_���ȉӏ��Ɠ���ւ�
            int n = UnityEngine.Random.Range(0, i + 1);
            int temp = deckCardIDList[i];
            deckCardIDList[i] = deckCardIDList[n];
            deckCardIDList[n] = temp;
        }
    }

    /// <summary>
    /// ���V���b�t������
    /// </summary>
    public async UniTask ReshuffleDeck()
    {
        await UIManager.instance.RunMessage(_RESHUFFLE_TEXT_ID.ToText());
        // �̂ĎD���f�b�L�ɖ߂�
        deckCardIDList.AddRange(discardCardIDList);
        discardCardIDList.Clear();
        // �f�b�L���V���b�t��
        ShuffleDeck();
    }

    /// <summary>
    /// �f�b�L����w�薇���h���[����
    /// </summary>
    /// <param name="drawCount"></param>
    public async UniTask DrawDeck(int drawCount)
    {
        for (int i = 0; i < drawCount; i++)
        {
            // �f�b�L���Ȃ��Ȃ烊�V���b�t��
            if (deckCardIDList.Count <= 0) await ReshuffleDeck();
            if (deckCardIDList.Count <= 0) return;
            handCardIDList.Add(deckCardIDList[0]);
            deckCardIDList.RemoveAt(0);
        }
    }

    /// <summary>
    /// ��D���ő喇���ɂȂ�悤�Ɉ���
    /// </summary>
    public async UniTask DrawDeckMax()
    {
        int drawCount = _HAND_MAX - handCardIDList.Count;
        await DrawDeck(drawCount);
    }

    /// <summary>
    /// ��D�̏��Ԏw��ŃJ�[�h���g�p
    /// </summary>
    /// <param name="handIndex"></param>
    /// <param name="useCharacter"></param>
    /// <returns></returns>
    public async UniTask<int> UseCard(int handIndex, Character useCharacter)
    {
        int cardID = handCardIDList[handIndex];
        // ��D����ꎞ�I�ɔj��
        handCardIDList.Remove(cardID);
        CardData useCard = CardManager.GetCard(cardID);
        if (useCard == null) return -1;
        // �C�x���g����
        EventContext context = new EventContext()
        {
            character = useCharacter,
            card = useCard
        };
        await EventManager.ExecuteEvent(useCard.eventID, context);
        // �R�C���ǉ�
        useCharacter.AddCoin(useCard.addCoin);
        // �J�[�h���̂ĎD�ɒǉ�
        discardCardIDList.Add(cardID);
        return useCard.advance;
    }

    /// <summary>
    /// ���Ԏw��Ŏ�D�̃J�[�h���̂Ă�
    /// </summary>
    /// <param name="handCount"></param>
    public void DiscardHandIndex(int handCount)
    {
        if (handCardIDList.Count <= handCount) return;
        discardCardIDList.Add(handCardIDList[handCount]);
        handCardIDList.RemoveAt(handCount);
    }

    /// <summary>
    /// ID�w��Ŏ�D�̃J�[�h���̂Ă�
    /// </summary>
    /// <param name="cardID"></param>
    public void DiscardHandID(int cardID)
    {
        handCardIDList.Remove(cardID);
        discardCardIDList.Add(cardID);
    }

    /// <summary>
    /// ��D�����ׂĎ̂Ă�
    /// </summary>
    public void DiscardHandAll()
    {
        discardCardIDList.AddRange(handCardIDList);
        handCardIDList.Clear();
    }

    /// <summary>
    /// �f�b�L����w�薇���̂Ă�
    /// </summary>
    /// <param name="discardCount"></param>
    public async UniTask DiscardDeck(int discardCount)
    {
        for (int i = 0; i < discardCount; i++)
        {
            // �f�b�L���Ȃ��Ȃ烊�V���b�t��
            if (deckCardIDList.Count <= 0) await ReshuffleDeck();
            discardCardIDList.Add(deckCardIDList[0]);
            deckCardIDList.RemoveAt(0);
        }
    }

    /// <summary>
    /// �w��ID�������J�[�h�ɉ�����
    /// </summary>
    /// <param name="ID"></param>
    public async UniTask AddCard(int ID)
    {
        discardCardIDList.Add(ID);
        possessCardIDList.Add(ID);

        // �X�^�[�J�[�h�Ȃ�UI�X�V
        CardData card = CardManager.GetCard(ID);
        if (!card.IsStar()) return;

        _AddStarCallback(1);
        await UIManager.instance.RunMessage(_GET_STAR_TEXT_ID.ToText());
    }

    /// <summary>
    /// ID�w��Ŏ�D�̃J�[�h��j��
    /// </summary>
    /// <param name="cardID"></param>
    public void RemoveHandID(int cardID)
    {
        possessCardIDList.Remove(cardID);
        handCardIDList.Remove(cardID);
    }

    /// <summary>
    /// ��D�������J�[�h����j��
    /// </summary>
    public void RemoveHandAll()
    {
        for (int i = 0, max = handCardIDList.Count; i < max; i++)
        {
            int handCardID = handCardIDList[i];
            possessCardIDList.Remove(handCardID);
            handCardIDList.Remove(handCardID);

            CardData card = CardManager.GetCard(handCardID);
            if (!card.IsStar()) continue;
            _LoseStarCallback(1);
        }
    }

    /// <summary>
    /// ��D�̃X�^�[�J�[�h��j��
    /// </summary>
    /// <returns></returns>
    public int RemoveHandStarCard()
    {
        for (int i = 0, max = handCardIDList.Count; i < max; i++)
        {
            int handCardID = handCardIDList[i];
            CardData card = CardManager.GetCard(handCardID);
            if (!card.IsStar()) continue;

            possessCardIDList.Remove(handCardID);
            handCardIDList.Remove(handCardID);
            _LoseStarCallback(1);

            return handCardID;
        }
        return -1;
    }

    /// <summary>
    /// �X�^�[�̐��𐔂���
    /// </summary>
    /// <returns></returns>
    public int CountStar()
    {
        int star = 0;
        for (int i = 0, max = possessCardIDList.Count; i < max; i++)
        {
            CardData card = CardManager.GetCard(possessCardIDList[i]);
            star += card.star;
        }
        return star;
    }

    /// <summary>
    /// �R�[���o�b�N��ݒ�
    /// </summary>
    /// <param name="setCallback"></param>
    public void SetCallback(Action<int> setAddStarCallback, Func<int, int> setLoseStarCallback)
    {
        _AddStarCallback = setAddStarCallback;
        _LoseStarCallback = setLoseStarCallback;
    }

    /// <summary>
    /// �f�b�L�̎w��ID�̃J�[�h��j������
    /// </summary>
    /// <param name="cardID"></param>
    public void RemoveDeckCard(int cardID)
    {
        // �������Ă���J�[�h��ID�����邩����
        int index = possessCardIDList.IndexOf(cardID);
        if (index == -1) return;
        possessCardIDList.RemoveAt(index);

        index = deckCardIDList.IndexOf(cardID);
        if (index == -1) return;
        deckCardIDList.RemoveAt(index);
    }
}
