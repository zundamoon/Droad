using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static GameEnum;

public class Event013_Shop : BaseEvent
{
    private const int _BRONZE_CARD_COUNT = 2;
    private const int _SILVER_CARD_COUNT = 2;
    private const int _GOLD_CARD_COUNT = 1;
    private const int _LEGENDARY_CARD_COUNT = 1;
    private const int _CARD_COUNT = 6;

    private int _cardID = -1;
    private bool _acceptInput = false;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // �\���J�[�h�𒊑I
        List<int> cardIDList = new List<int>(_CARD_COUNT);
        GetRandCard(ref cardIDList);

        // Ui�ɃJ�[�h����n��
        await UIManager.instance.SetRemovaItem(character.possessCard.deckCardIDList);
        await UIManager.instance.SetSelectCallback((cardID, isRemove) =>
        {
            if (isRemove) character.possessCard.RemoveDeckCard(cardID);
            else character.possessCard.AddCard(cardID);
        });
        await UIManager.instance.SetBuyItem(cardIDList);
        // �V���b�vUI��\��
        await UIManager.instance.OpenShop();
    }

    /// <summary>
    /// �V���b�v�̃J�[�h���X�g���擾
    /// </summary>
    /// <param name="cardIDList"></param>
    private void GetRandCard(ref List<int> cardIDList)
    {
        // �u�����Y
        for (int i = 0; i < _BRONZE_CARD_COUNT; i++)
        {
            int cardID = CardManager.GetRandRarityCard(Rarity.BRONZE);
            cardIDList.Add(cardID);
        }
        // �V���o�[
        for (int i = 0; i < _SILVER_CARD_COUNT; i++)
        {
            int cardID = CardManager.GetRandRarityCard(Rarity.SILVER);
            cardIDList.Add(cardID);
        }
        // �S�[���h
        for (int i = 0; i < _GOLD_CARD_COUNT; i++)
        {
            int cardID = CardManager.GetRandRarityCard(Rarity.GOLD);
            cardIDList.Add(cardID);
        }
        // ���W�F���h
        for (int i = 0; i < _LEGENDARY_CARD_COUNT; i++)
        {
            int cardID = CardManager.GetRandRarityCard(Rarity.LEGENDARY);
            cardIDList.Add(cardID);
        }
    }
}
