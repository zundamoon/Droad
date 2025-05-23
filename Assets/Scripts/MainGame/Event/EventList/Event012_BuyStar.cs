using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.TextCore.Text;

public class Event012_BuyStar : BaseEvent
{
    private readonly int[] _STAR_CARD_ID_LIST = { 24, 25, 26, 27, 28, 29 };

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // 表示カードを抽選
        List<int> cardIDList = new List<int>(1);
        int starID = _STAR_CARD_ID_LIST[Random.Range(0, _STAR_CARD_ID_LIST.Length)];
        cardIDList.Add(starID);

        // Uiにカード情報を渡す
        await UIManager.instance.RemoveAllShopItem();
        await UIManager.instance.SetBuyItem(cardIDList);
        await UIManager.instance.SetSelectCallback(async (cardID, isRemove) =>
        {
            CardData card = CardManager.GetCard(cardID);
            if (!await character.Pay(card.price)) return;
            await character.possessCard.AddCardDiscard(cardID);
            await UIManager.instance.RemoveShopItem(cardID, isRemove);
        });
        // ショップUIを表示
        await UIManager.instance.OpenShop();
    }
}
