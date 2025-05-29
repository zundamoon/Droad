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
        Square square = context.square;
        if (character == null || square == null) return;

        // 表示カードを抽選
        List<int> cardIDList = new List<int>(1);
        int starID = _STAR_CARD_ID_LIST[Random.Range(0, _STAR_CARD_ID_LIST.Length)];
        cardIDList.Add(starID);

        // Uiにカード情報を渡す
        await UIManager.instance.RemoveAllShopItem();
        await UIManager.instance.SetBuyItem(cardIDList);
        await UIManager.instance.SetSelectCallback(async (cardID, isRemove) =>
        {
            CardData card = CardManager.instance.GetCard(cardID);
            if (!await character.Pay(card.price)) return;
            await UIManager.instance.RemoveShopItem(cardID, isRemove);
            await character.possessCard.AddCardDiscard(cardID);
            StageManager.instance.DecideStarSquare();
            square.ChangeStarSquare();
        });
        // ショップUIを表示
        await UIManager.instance.OpenShop();
    }
}
