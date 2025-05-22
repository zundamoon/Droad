using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Event021_PassExchangeHand : BaseEvent
{
    private const int _TEXT_ID = 122;

    public override async UniTask ExecuteEvent(EventContext context, int param)
    {
        if (context == null) return;

        Character character = context.character;
        if (character == null) return;

        // ƒvƒŒƒCƒ„[‚ÌˆÚ“®Œã‚Ìˆ—‚ÉÝ’è
        character.SetAfterMoveEvent(async (targetCharacterList) =>
        {
            int targetCount = targetCharacterList.Count;
            if (targetCount <= 0) return;

            Character target = targetCharacterList[0];
            // ŽèŽD‚ðŒðŠ·
            await ExchangeHand(character, target);

            int playerID = targetCharacterList[0].playerID;
            await UIManager.instance.RunMessage(string.Format(_TEXT_ID.ToText(), playerID + 1));
        });
        await UniTask.CompletedTask;
    }

    private async UniTask ExchangeHand(Character sourceCharacter, Character targetCharacter)
    {
        // ŽèŽD‚ðŒðŠ·
        List<int> targetHand = targetCharacter.possessCard.handCardIDList;
        targetCharacter.possessCard.RemoveHandAll();
        List<int> sourceHand = sourceCharacter.possessCard.handCardIDList;
        for (int i = 0, max = sourceHand.Count; i < max; i++)
        {
            await targetCharacter.possessCard.AddCardHand(sourceHand[i]);
        }
        sourceCharacter.possessCard.RemoveHandAll();
        for (int i = 0, max = targetHand.Count; i < max; i++)
        {
            await sourceCharacter.possessCard.AddCardHand(targetHand[i]);
        }
    }
}
