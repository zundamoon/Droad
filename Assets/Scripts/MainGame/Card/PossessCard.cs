using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int _DEFAULT_DECK_MAX = 12;

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
            int n = Random.Range(0, i + 1);
            int temp = deckCardIDList[i];
            deckCardIDList[i] = deckCardIDList[n];
            deckCardIDList[n] = temp;
        }
    }

    /// <summary>
    /// ���V���b�t������
    /// </summary>
    public void ReshuffleDeck()
    {
        // �̂ĎD���f�b�L�ɖ߂�
        discardCardIDList.AddRange(deckCardIDList);
        discardCardIDList.Clear();
        // �f�b�L���V���b�t��
        ShuffleDeck();
    }

    /// <summary>
    /// �f�b�L����w�薇���h���[����
    /// </summary>
    /// <param name="drawCount"></param>
    public void DrawDeck(int drawCount)
    {
        for (int i = 0; i < drawCount; i++)
        {
            // �f�b�L���Ȃ��Ȃ烊�V���b�t��
            if (deckCardIDList.Count <= 0) ReshuffleDeck();
            handCardIDList.Add(deckCardIDList[0]);
            deckCardIDList.RemoveAt(0);
        }
    }

    /// <summary>
    /// ��D�̎w��̃J�[�h���̂Ă�
    /// </summary>
    /// <param name="handCount"></param>
    public void DiscardHand(int handCount)
    {
        if (handCardIDList.Count <= handCount) return;
        discardCardIDList.Add(handCardIDList[handCount]);
        handCardIDList.RemoveAt(handCount);
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
    public void DiscardDeck(int discardCount)
    {
        for (int i = 0; i < discardCount; i++)
        {
            // �f�b�L���Ȃ��Ȃ烊�V���b�t��
            if (deckCardIDList.Count <= 0) ReshuffleDeck();
            discardCardIDList.Add(deckCardIDList[0]);
            deckCardIDList.RemoveAt(0);
        }
    }
}
