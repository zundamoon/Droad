using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EventManager
{
    // �C�x���g�̃��X�g
    public static List<IEvent> eventList { get; private set; } = null;
    // �����̃��X�g
    public static List<ICondition> conditionList { get; private set; } = null;

    private const int _UNCOMPLETE_CONDITION_TEXT_ID = 130;

    public static void Init()
    {
        EventInit();
        ConditionInit();
    }

    /// <summary>
    /// �C�x���g���X�g�̏�����
    /// </summary>
    private static void EventInit()
    {
        eventList = new List<IEvent>();
        eventList.Add(new Event000_DiscardHand());
        eventList.Add(new Event001_PassStealCoin());
        eventList.Add(new Event002_Reshuffle());
        eventList.Add(new Event003_DiscardDeckTop());
        eventList.Add(new Event004_CancelNextSquareEvent());
        eventList.Add(new Event005_DiscardDeckTopExe());
        eventList.Add(new Event006_AddCoin());
        eventList.Add(new Event007_ReshuffleAll());
        eventList.Add(new Event008_LoseCoin());
        eventList.Add(new Event009_TurningRoute());
        eventList.Add(new Event010_LuckyEvent());
        eventList.Add(new Event011_UnluckyEvent());
        eventList.Add(new Event012_BuyStar());
        eventList.Add(new Event013_Shop());
        eventList.Add(new Event014_DiscardHandAll());
        eventList.Add(new Event015_NoPassAddCoin());
        eventList.Add(new Event016_RepeatNextSquareEvent());
        eventList.Add(new Event017_PassDiscardHand());
        eventList.Add(new Event018_PassStealStar());
        eventList.Add(new Event019_Advance123());
        eventList.Add(new Event020_Advance456());
        eventList.Add(new Event021_PassExchangeHand());
        eventList.Add(new Event022_DiscardHandAdvance());
        eventList.Add(new Event023_ExecuteDiscardCard());
        eventList.Add(new Event024_ExecuteHandCard());
        eventList.Add(new Event025_SameAdvanceAddCoin());
        eventList.Add(new Event026_ReturnDiscardToDeck());
        eventList.Add(new Event027_LoseHalfCoinToAdvance());
        eventList.Add(new Event028_GiftCard());
        eventList.Add(new Event029_DoubleCoin());
        eventList.Add(new Event030_LoseCoinEveryone());
        eventList.Add(new Event031_StealCoinEveryone());
        eventList.Add(new Event032_LoseHalfCoin());

    }

    /// <summary>
    /// �������X�g�̏�����
    /// </summary>
    private static void ConditionInit()
    {
        conditionList = new List<ICondition>();
        conditionList.Add(new Condition000_CoinHigher());
        conditionList.Add(new Condition001_CoinLower());
        conditionList.Add(new Condition002_HandStarHigher());
        conditionList.Add(new Condition003_HandEvenAll());
        conditionList.Add(new Condition004_HandOddAll());
        conditionList.Add(new Condition005_HandAdvanceHigherAll());
        conditionList.Add(new Condition006_HandAdvanceLowerAll());
        conditionList.Add(new Condition007_HandAdvanceSameAll());
        conditionList.Add(new Condition008_HandRaritySameAll());
        conditionList.Add(new Condition009_DeckHigher());
        conditionList.Add(new Condition010_DeckLower());
        conditionList.Add(new Condition011_DiscardHigher());
        conditionList.Add(new Condition012_DiscardLower());
        conditionList.Add(new Condition013_RankTop());
        conditionList.Add(new Condition014_RankNoTop());
        conditionList.Add(new Condition015_RankLowest());
        conditionList.Add(new Condition016_RankNoLowest());
        conditionList.Add(new Condition017_HandAdvanceSumHigher());
        conditionList.Add(new Condition018_HandAdvanceSumLower());
        conditionList.Add(new Condition019_HandAdvanceSumSame());
        conditionList.Add(new Condition020_Probability());
        conditionList.Add(new Condition021_HandAdvanceSame());
        conditionList.Add(new Condition022_HandRarityHigherSilver());
        conditionList.Add(new Condition023_LoseCoin());
        conditionList.Add(new Condition024_LoseStar());
    }

    /// <summary>
    /// �C�x���g�̎��s
    /// </summary>
    /// <param name="eventID"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async UniTask ExecuteEvent(int eventID, EventContext context = null)
    {
        var eventMaster = EventMasterUtility.GetEventMaster(eventID);
        if (eventMaster == null) return;

        // �����B���ł��Ȃ��Ȃ珈�����Ȃ�
        int conditionID = eventMaster.conditionID;
        if (conditionID >= 0 && !await IsCompleteCondition(conditionID, context))
        {
            await UIManager.instance.RunMessage(_UNCOMPLETE_CONDITION_TEXT_ID.ToText());
            return;
        }

        int eventIndex = eventMaster.eventType;
        int eventParam = eventMaster.param[0];
        await eventList[eventIndex].ExecuteEvent(context, eventParam);
    }

    /// <summary>
    /// ������B���������ǂ���
    /// </summary>
    /// <returns></returns>
    private static async UniTask<bool> IsCompleteCondition(int conditionID, EventContext context)
    {
        // ����ID��������擾
        var conditionMaster = ConditionMasterUtility.GetConditionMaster(conditionID);
        if (conditionMaster == null) return false;

        int conditionType = conditionMaster.type;
        int conditionParam = conditionMaster.param;

        return await conditionList[conditionType].IsCompleteCondition(context, conditionParam);
    }
}
