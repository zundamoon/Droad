using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager
{
    public List<Card> cardList { get; private set; } = null;

    public void Init()
    {
        // �}�X�^�[�f�[�^����J�[�h�𐶐�
        cardList = new List<Card>();
        
    }
}
