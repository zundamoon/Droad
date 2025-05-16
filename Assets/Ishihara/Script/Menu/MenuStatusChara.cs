using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuStatusChara : MenuStatusItem
{
    [SerializeField] 
    private TextMeshProUGUI _coinText = null;
    [SerializeField]
    private TextMeshProUGUI _starText = null;

    [SerializeField]
    private Image _bgImage = null;

    [SerializeField]
    private List<TextMeshProUGUI> _charaCards = null;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        isChara = true;
    }

    /// <summary>
    /// キャラの設定
    /// </summary>
    /// <param name="setChara"></param>
    public void SetChara(Character setChara)
    {
        // キャラの設定
        var chara = setChara;
        Color charaColor = chara.playerColor;
        int charaCion = chara.coins;
        int charaStar = chara.stars;

        _coinText.text = charaCion.ToString();
        _starText.text = charaStar.ToString();
        _bgImage.color = charaColor;

        // カードの設定
        for (int i = 0; i < _charaCards.Count; i++)
        {
            if (i < chara.possessCard.handCardIDList.Count)
            {
                int cardID = chara.possessCard.handCardIDList[i];
                CardData cardData = CardManager.GetCard(cardID);
                _charaCards[i].text = cardData.advance.ToString();
            }
            else
            {
                _charaCards[i].text = "";
            }
        }
    }
}
