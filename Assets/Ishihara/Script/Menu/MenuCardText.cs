using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MenuCardText : BaseMenu
{
    [SerializeField]
    private TextMeshProUGUI _cardtext = null;

    [SerializeField]
    private TextMeshProUGUI _advanceText = null;

    [SerializeField]
    private TextMeshProUGUI _coinText = null;

    [SerializeField]
    private TextMeshProUGUI _nameText = null;

    private const int _ADVANCE_TEXT_ID = 200;
    private const int _CON_TEXT_ID = 201;


    public async UniTask SetText(int cardID)
    {
        int ID = cardID;
        // �J�[�h���擾
        CardData card = CardManager.instance.GetCard(ID);
        if (card == null) return;
        _nameText.text = card.nameID.ToText();
        _advanceText.text = string.Format(_ADVANCE_TEXT_ID.ToText(), card.advance);
        _coinText.text = string.Format(_CON_TEXT_ID.ToText(), card.addCoin);
        Entity_EventData.Param param = EventMasterUtility.GetEventMaster(card.eventID);
        if (param == null)
        {
            _cardtext.text = "";
            return;
        }
        int eventTextID = param.textID;
        int[] paramList = param.param;
        _cardtext.text = string.Format(eventTextID.ToText(), paramList[0], paramList[1]);
        await UniTask.CompletedTask;
    }

    public override UniTask Close()
    {
        // ����������
        _cardtext.text = string.Empty;
        return base.Close();
    }
}
