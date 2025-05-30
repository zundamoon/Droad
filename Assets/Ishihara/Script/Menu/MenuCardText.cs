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
    private const int _CONDITION_TEXT_ID = 202;
    private const int _EVENT_TEXT_ID = 203;

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
        int conditionID = param.conditionID;
        string conditionText = string.Format(_CONDITION_TEXT_ID.ToText() + "�Ȃ�"); var conditionMaster = ConditionMasterUtility.GetConditionMaster(conditionID);
        if (conditionID != -1)
            conditionText = string.Format(_CONDITION_TEXT_ID.ToText() + string.Format(conditionMaster.textID.ToText(), conditionMaster.param));
        int eventTextID = param.textID;
        int[] paramList = param.param;
        string eventText = string.Format(_EVENT_TEXT_ID.ToText() + eventTextID.ToText(), paramList[0], paramList[1]);
        _cardtext.text = string.Format(conditionText + '\n' + eventText);
        await UniTask.CompletedTask;
    }

    public override UniTask Close()
    {
        // ����������
        _cardtext.text = string.Empty;
        return base.Close();
    }
}
