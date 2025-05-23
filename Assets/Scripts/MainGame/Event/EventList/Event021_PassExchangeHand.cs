using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        // ƒvƒŒƒCƒ„[‚ÌˆÚ“®Œã‚Ìˆ—‚Éİ’è
        character.SetAfterMoveEvent(async (targetCharacterList) =>
        {
            int targetCount = targetCharacterList.Count;
            if (targetCount <= 0) return;

            Character target = targetCharacterList[0];
            // èD‚ğŒğŠ·
            await ExchangeHand(character, target);

            int playerID = targetCharacterList[0].playerID;
            await UIManager.instance.RunMessage(string.Format(_TEXT_ID.ToText(), playerID + 1));
        });
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// èD‚ğŒğŠ·‚·‚é
    /// </summary>
    /// <param name="sourceCharacter"></param>
    /// <param name="targetCharacter"></param>
    /// <returns></returns>
    private async UniTask ExchangeHand(Character sourceCharacter, Character targetCharacter)
    {
        // èD‚ğŒğŠ·
        List<int> targetHand = targetCharacter.possessCard.handCardIDList.ToList();
        List<int> sourceHand = sourceCharacter.possessCard.handCardIDList.ToList();
        targetCharacter.possessCard.RemoveHandAll();
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
