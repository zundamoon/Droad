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

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // 表示カードを抽選
        List<int> cardIDList = new List<int>(_CARD_COUNT);
        GetRandCard(ref cardIDList);

        // Uiにカード情報を渡す
        await UIManager.instance.SetSelectCallback((cardID, isRemove) =>
        {
            if (isRemove) character.possessCard.RemoveDeckCard(cardID);
            else character.possessCard.AddCard(cardID);
        });
        await UIManager.instance.SetBuyItem(cardIDList);
        // ショップUIを表示
        await UIManager.instance.OpenShop();
        // 入力待ち
        while (true)
        {
            await UniTask.DelayFrame(1);
        }
    }

    /// <summary>
    /// ショップのカードリストを取得
    /// </summary>
    /// <param name="cardIDList"></param>
    private void GetRandCard(ref List<int> cardIDList)
    {
        // ブロンズ
        for (int i = 0; i < _BRONZE_CARD_COUNT; i++)
        {
            int cardID = CardManager.GetRandRarityCard(Rarity.BRONZE);
            cardIDList.Add(cardID);
        }
        // シルバー
        for (int i = 0; i < _SILVER_CARD_COUNT; i++)
        {
            int cardID = CardManager.GetRandRarityCard(Rarity.SILVER);
            cardIDList.Add(cardID);
        }
        // ゴールド
        for (int i = 0; i < _GOLD_CARD_COUNT; i++)
        {
            int cardID = CardManager.GetRandRarityCard(Rarity.GOLD);
            cardIDList.Add(cardID);
        }
        // レジェンド
        for (int i = 0; i < _LEGENDARY_CARD_COUNT; i++)
        {
            int cardID = CardManager.GetRandRarityCard(Rarity.LEGENDARY);
            cardIDList.Add(cardID);
        }
    }
}
