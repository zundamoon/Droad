using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

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

    private Action<int> _AddStarCallback = null;
    private Func<int, int> _LoseStarCallback = null;
    private Action _CardCallback = null;

    private const int _DEFAULT_DECK_MAX = 12;
    private const int _HAND_MAX = 4;
    private const int _GET_STAR_TEXT_ID = 106;
    private const int _RESHUFFLE_TEXT_ID = 107;
    private const int _GET_CARD_TEXT_ID = 127;

    public void Init()
    {
        possessCardIDList = new List<int>(_DEFAULT_DECK_MAX);
        deckCardIDList = new List<int>(_DEFAULT_DECK_MAX);
        handCardIDList = new List<int>(_DEFAULT_DECK_MAX);
        discardCardIDList = new List<int>(_DEFAULT_DECK_MAX);

        // ������D�ݒ�
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 1; j++)
            {
                deckCardIDList.Add(i);
                possessCardIDList.Add(i);
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
            _CardCallback();
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
        // �R�C���ǉ�
        useCharacter.AddCoin(useCard.addCoin);
        // �C�x���g����
        EventContext context = new EventContext()
        {
            character = useCharacter,
            card = useCard
        };
        await EventManager.ExecuteEvent(useCard.eventID, context);
        // �J�[�h���̂ĎD�ɒǉ�
        discardCardIDList.Add(cardID);
        _CardCallback();
        return context.card.advance;
    }

    /// <summary>
    /// ���Ԏw��Ŏ�D�̃J�[�h���̂Ă�
    /// </summary>
    /// <param name="handCount"></param>
    public async UniTask DiscardHandIndex(int handCount)
    {
        if (handCardIDList.Count <= handCount) return;
        discardCardIDList.Add(handCardIDList[handCount]);
        await UIManager.instance.HandDiscard(handCount);
        handCardIDList.RemoveAt(handCount);
        _CardCallback();
    }

    /// <summary>
    /// ID�w��Ŏ�D�̃J�[�h���̂Ă�
    /// </summary>
    /// <param name="cardID"></param>
    public async UniTask DiscardHandID(int cardID)
    {
        int handIndex = handCardIDList.IndexOf(cardID);
        await UIManager.instance.HandDiscard(handIndex);
        discardCardIDList.Add(cardID);
        handCardIDList.Remove(cardID);
        _CardCallback();
    }

    /// <summary>
    /// ��D�����ׂĎ̂Ă�
    /// </summary>
    public async UniTask DiscardHandAll()
    {
        // ��D�̃C���f�b�N�X���t���ɏ�������
        for (int i = handCardIDList.Count - 1; i >= 0; i--)
        {
            await DiscardHandIndex(i);
        }
    }

    /// <summary>
    /// ��D����w�薇�������_���Ɏ̂Ă�
    /// </summary>
    /// <param name="discardCount"></param>
    public void DiscardRandHand(int discardCount)
    {
        for (int i = 0; i < discardCount; i++)
        {
            int handCount = handCardIDList.Count;
            if (handCount <= 0) return;

            int randIndex = UnityEngine.Random.Range(0, handCount);
            DiscardHandIndex(randIndex);
        }
    }

    /// <summary>
    /// �f�b�L����w�薇���̂Ă�
    /// </summary>
    /// <param name="discardCount"></param>
    public async UniTask DiscardDeckTop(int discardCount)
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
    /// �w��ID���̂ĎD�ɉ�����
    /// </summary>
    /// <param name="ID"></param>
    public async UniTask AddCardDiscard(int ID)
    {
        discardCardIDList.Add(ID);
        possessCardIDList.Add(ID);
        _CardCallback();
        int cardNameID = CardManager.GetCard(ID).nameID;
        await UIManager.instance.RunMessage(string.Format(_GET_CARD_TEXT_ID.ToText(), cardNameID.ToText()));

        // �X�^�[�J�[�h�Ȃ�UI�X�V
        CardData card = CardManager.GetCard(ID);
        if (!card.IsStar()) return;

        _AddStarCallback(1);
    }

    /// <summary>
    /// �w��ID����D�ɉ�����
    /// </summary>
    /// <param name="ID"></param>
    public async UniTask AddCardHand(int ID)
    {
        handCardIDList.Add(ID);
        possessCardIDList.Add(ID);
        UIManager.instance.HandDraw(ID);
        _CardCallback();

        // �X�^�[�J�[�h�Ȃ�UI�X�V
        CardData card = CardManager.GetCard(ID);
        if (!card.IsStar()) return;

        _AddStarCallback(1);
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// ID�w��Ŏ�D�̃J�[�h��j��
    /// </summary>
    /// <param name="cardID"></param>
    public void RemoveHandID(int cardID)
    {
        possessCardIDList.Remove(cardID);
        int handIndex = handCardIDList.IndexOf(cardID);
        UIManager.instance.HandDiscard(handIndex);
        handCardIDList.Remove(cardID);
        _CardCallback();
    }

    /// <summary>
    /// ��D�������J�[�h����j��
    /// </summary>
    public void RemoveHandAll()
    {
        var handCardIDTemp = new List<int>(handCardIDList);
        for (int i = 0, max = handCardIDList.Count; i < max; i++)
        {
            int handCardID = handCardIDTemp[i];
            possessCardIDList.Remove(handCardID);
            int handIndex = handCardIDList.IndexOf(handCardID);
            UIManager.instance.HandDiscard(handIndex);
            handCardIDList.Remove(handCardID);
            _CardCallback();

            CardData card = CardManager.GetCard(handCardID);
            if (!card.IsStar()) continue;
            _LoseStarCallback(1);
        }
    }

    /// <summary>
    /// �������Ă���X�^�[�J�[�h��j��
    /// </summary>
    /// <returns></returns>
    public int RemoveRandomStarCard()
    {
        // �X�^�[�J�[�hID���擾
        int max = possessCardIDList.Count;
        List<int> starIDList = new List<int>(max);
        for (int i = 0; i < max; i++)
        {
            if (!CardManager.GetCard(possessCardIDList[i]).IsStar()) continue;

            starIDList.Add(i);
        }
        int starCount = starIDList.Count;
        if (starCount <= 0) return -1;
        int randomID = UnityEngine.Random.Range(0, starCount);

        // �w��ID���������O����
        RemoveCardID(randomID);
        possessCardIDList.Remove(randomID);

        return randomID;
    }

    /// <summary>
    /// ID�w��̃J�[�h���O
    /// </summary>
    /// <param name="cardID"></param>
    private void RemoveCardID(int cardID)
    {
        if (handCardIDList.Remove(cardID)) return;
        if (discardCardIDList.Remove(cardID)) return;
        if (deckCardIDList.Remove(cardID)) return;
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
    public void SetCallback(Action<int> setAddStarCallback, Func<int, int> setLoseStarCallback, Action setCardCallback)
    {
        _AddStarCallback = setAddStarCallback;
        _LoseStarCallback = setLoseStarCallback;
        _CardCallback = setCardCallback;
    }

    /// <summary>
    /// �f�b�L�̎w��ID�̃J�[�h��j������
    /// </summary>
    /// <param name="cardID"></param>
    public async UniTask RemoveDeckCard(int cardID)
    {
        possessCardIDList.Remove(cardID);
        deckCardIDList.Remove(cardID);

        if (deckCardIDList.Count > 0) ShuffleDeck();
        else await ReshuffleDeck();
    }

    /// <summary>
    /// ID�w��Ŏ̂ĎD����f�b�L�ɖ߂�
    /// </summary>
    /// <param name="cardID"></param>
    public void ReturnDiscardToDeck(int cardID)
    {
        discardCardIDList.Remove(cardID);
        deckCardIDList.Add(cardID);
        ShuffleDeck();
    }
}
