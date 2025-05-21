using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public int ID { get; private set; } = -1;
    public int nameID { get; private set; } = -1;
    public int advance { get; private set; } = 0;
    public int addCoin { get; private set; } = 0;
    public int star { get; private set; } = 0;
    public GameEnum.Rarity rarity { get; private set; } = 0;
    public int eventID { get; private set; } = -1;
    public int price { get; private set; } = 0;
    
    public void Init(int setID, int setNameID, int setAdvance, int setAddCoin, int setStar, GameEnum.Rarity setRarity, int setEventID, int setPrice)
    {
        ID = setID;
        nameID = setNameID;
        advance = setAdvance;
        addCoin = setAddCoin;
        star = setStar;
        rarity = setRarity;
        eventID = setEventID;
        price = setPrice;
    }

    /// <summary>
    /// �X�^�[�J�[�h���ۂ�
    /// </summary>
    /// <returns></returns>
    public bool IsStar() { return star != 0; }
}
