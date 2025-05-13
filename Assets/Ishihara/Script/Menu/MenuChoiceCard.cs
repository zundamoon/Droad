using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuChoiceCard : MonoBehaviour
{
    [SerializeField]
    private CardObject _cardObject = null;

    [SerializeField]
    private Button _button = null;

    public void SetCard(int ID)
    {
        _cardObject.SetCard(ID);
    }

    public void SetButtonAction(System.Action action)
    {
        _button.onClick.AddListener(() => action());
    }
}
