using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessCard
{
    /// <summary>
    /// 所持カードリスト
    /// </summary>
    public List<int> possessCardIDList { get; private set; } = null;
    /// <summary>
    /// デッキカードリスト
    /// </summary>
    public List<int> deckCardIDList { get; private set; } = null;
    /// <summary>
    /// 手札カードリスト
    /// </summary>
    public List<int> handCardIDList { get; private set; } = null;
    /// <summary>
    /// 捨てカードリスト
    /// </summary>
    public List<int> discardCardIDList { get; private set; } = null;
    public List<int> starCardIDList { get; private set; } = null;

    private const int _DEFAULT_DECK_MAX = 12;
    private const int _HAND_MAX = 4;

    public void Init()
    {
        possessCardIDList = new List<int>(_DEFAULT_DECK_MAX);
        deckCardIDList = new List<int>(_DEFAULT_DECK_MAX);
        handCardIDList = new List<int>(_DEFAULT_DECK_MAX);
        discardCardIDList = new List<int>(_DEFAULT_DECK_MAX);

        // 初期手札設定
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
    /// デッキをシャッフルする
    /// </summary>
    public void ShuffleDeck()
    {
        int deckCount = deckCardIDList.Count;
        for (int i = deckCount - 1; i > 0; i--)
        {
            // ランダムな箇所と入れ替え
            int n = Random.Range(0, i + 1);
            int temp = deckCardIDList[i];
            deckCardIDList[i] = deckCardIDList[n];
            deckCardIDList[n] = temp;
        }
    }

    /// <summary>
    /// リシャッフルする
    /// </summary>
    public void ReshuffleDeck()
    {
        // 捨て札をデッキに戻す
        deckCardIDList.AddRange(discardCardIDList);
        discardCardIDList.Clear();
        // デッキをシャッフル
        ShuffleDeck();
    }

    /// <summary>
    /// デッキから指定枚数ドローする
    /// </summary>
    /// <param name="drawCount"></param>
    public void DrawDeck(int drawCount)
    {
        for (int i = 0; i < drawCount; i++)
        {
            // デッキがないならリシャッフル
            if (deckCardIDList.Count <= 0) ReshuffleDeck();
            handCardIDList.Add(deckCardIDList[0]);
            deckCardIDList.RemoveAt(0);
        }
    }

    /// <summary>
    /// 手札が最大枚数になるように引く
    /// </summary>
    public void DrawDeckMax()
    {
        int drawCount = _HAND_MAX - handCardIDList.Count;
        DrawDeck(drawCount);
    }

    /// <summary>
    /// 手札の指定のカードを捨てる
    /// </summary>
    /// <param name="handCount"></param>
    public void DiscardHand(int handCount)
    {
        if (handCardIDList.Count <= handCount) return;
        discardCardIDList.Add(handCardIDList[handCount]);
        handCardIDList.RemoveAt(handCount);
    }

    /// <summary>
    /// 手札をすべて捨てる
    /// </summary>
    public void DiscardHandAll()
    {
        discardCardIDList.AddRange(handCardIDList);
        handCardIDList.Clear();
    }

    /// <summary>
    /// デッキから指定枚数捨てる
    /// </summary>
    /// <param name="discardCount"></param>
    public void DiscardDeck(int discardCount)
    {
        for (int i = 0; i < discardCount; i++)
        {
            // デッキがないならリシャッフル
            if (deckCardIDList.Count <= 0) ReshuffleDeck();
            discardCardIDList.Add(deckCardIDList[0]);
            deckCardIDList.RemoveAt(0);
        }
    }

    /// <summary>
    /// 指定IDを所持カードに加える
    /// </summary>
    /// <param name="ID"></param>
    public void AddCard(int ID)
    {
        discardCardIDList.Add(ID);
        possessCardIDList.Add(ID);

        // スターカードならUI更新
        CardData card = CardManager.GetCard(ID);
        if (!card.IsStar()) return;


    }

    /// <summary>
    /// 手札を所持カードから破棄
    /// </summary>
    public void RemoveHandAll()
    {
        for (int i = 0, max = handCardIDList.Count; i < max; i++)
        {
            int handCardID = handCardIDList[i];
            possessCardIDList.Remove(handCardID);
            handCardIDList.Remove(handCardID);
        }
    }

    /// <summary>
    /// 手札のスターカードを破棄
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

            return handCardID;
        }
        return -1;
    }

    /// <summary>
    /// スターの数を数える
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
}
