using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

using static CommonModule;
using static GameConst;

public class TurnProcessor
{
    private List<int> _playerOrder = null;
    private bool _acceptEnd = false;
    private int _handIndex = -1;

    public void Init()
    {
        _playerOrder = new List<int>(PLAYER_MAX);
    }

    /// <summary>
    /// 各ターンの処理
    /// </summary>
    public async UniTask TurnProc()
    {
        // ドロー
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(i);
            character.possessCard.DrawDeckMax();
        }

        // 手番決め
        await DesidePlayerOrder();

        // 各手番
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            int orderIndex = _playerOrder[i];
            Character character = CharacterManager.instance.GetCharacter(orderIndex);
            await EachTurn(character);
        }
    }

    /// <summary>
    /// 手番を決める
    /// </summary>
    private async UniTask DesidePlayerOrder()
    {
        _playerOrder.Clear();
        List<int> playCardList = new List<int>(PLAYER_MAX);
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(i);
            // 手札の選択
            // UIの表示
            await UIManager.instance.OpenHandArea(character.possessCard);
            UIManager.instance.StartHandAccept();
            while (!_acceptEnd)
            {
                await UniTask.DelayFrame(1);
            }
            _acceptEnd = false;
            int playCardCount = GetOrderCount(_handIndex, character);
            playCardList.Add(playCardCount);
        }
        // 出されたカードから順番を決める
        while (_playerOrder.Count < PLAYER_MAX)
        {
            // 最大値のインデックスを取得
            int maxValue = playCardList.Max();
            int playCount = playCardList.Count;
            List<int> indexList = new List<int>(playCount);
            for (int i = 0; i < playCardList.Count; i++)
            {
                if (playCardList[i] != maxValue) continue;
                indexList.Add(i);
                playCardList[i] = -1;
            }
            // 複数ならランダムに決定
            while (indexList.Count > 0)
            {
                int index = Random.Range(0, indexList.Count);
                _playerOrder.Add(indexList[index]);
                indexList.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// 手札番号を指定し順番の数字を取得
    /// </summary>
    /// <param name="handIndex"></param>
    /// <returns></returns>
    private int GetOrderCount(int handIndex, Character playCharacter)
    {
        PossessCard possess = playCharacter.possessCard;
        int ID = possess.handCardIDList[handIndex];
        possess.DiscardHand(handIndex);
        return CardManager.GetCard(ID).advance;
    }

    /// <summary>
    /// キャラクターのターン処理
    /// </summary>
    /// <param name="turnCharacter"></param>
    private async UniTask EachTurn(Character turnCharacter)
    {
        if (turnCharacter == null) return;

        // UI表示
        await UIManager.instance.OpenHandArea(turnCharacter.possessCard);
        UIManager.instance.StartHandAccept();
        // 手札が使われるまで待機
        while (!_acceptEnd)
        {
            await UniTask.DelayFrame(1);
        }
        _acceptEnd = false;

        // カードの使用
        int advanceValue = await UseCard(_handIndex, turnCharacter);
        if (advanceValue <= 0) return;

        // キャラクターを動かす
        List<Character> targetCharacterList = new List<Character>(PLAYER_MAX);
        await MoveCharacter(advanceValue, turnCharacter, targetCharacterList);

        // 移動後処理を実行
        turnCharacter.ExecuteAfterMoveEvent(targetCharacterList);

        // イベント可能でなければ終了
        if (!turnCharacter.CanEvent()) return;
        await ExcuteSquareEvent(turnCharacter);
    }

    /// <summary>
    /// カードを使い、進マス数を返す
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="useCharacter"></param>
    /// <returns></returns>
    private async UniTask<int> UseCard(int handIndex, Character useCharacter)
    {
        // カードのIDから処理を実行
        PossessCard possess = useCharacter.possessCard;
        int cardID = possess.handCardIDList[handIndex];
        possess.DiscardHand(handIndex);
        CardData card = CardManager.GetCard(cardID);
        if (card == null) return -1;
        // イベント処理
        await EventManager.ExecuteEvent(useCharacter, card.eventID);
        // コイン追加
        useCharacter.AddCoin(card.addCoin);
        return card.advance;
    }

    /// <summary>
    /// キャラクターの移動
    /// </summary>
    /// <param name="advanceValue"></param>
    /// <param name="moveCharacter"></param>
    /// <param name="targetCharacterList"></param>
    /// <returns></returns>
    private async UniTask MoveCharacter(int advanceValue, Character moveCharacter, List<Character> targetCharacterList)
    {
        StageManager stageManager = StageManager.instance;
        // 進む分だけ動く
        for (int i = 0; i < advanceValue; i++)
        {
            // 移動先のマスを取得
            StagePosition nextPosition = stageManager.CheckNextPosition(moveCharacter.position);
            Vector3 movePosition = stageManager.GetPosition(nextPosition);

            // 移動
            await moveCharacter.Move(movePosition);
            moveCharacter.position = nextPosition;
            // マス上にいる他キャラを検出
            for (int j = 0; j < PLAYER_MAX; j++)
            {
                Character target = CharacterManager.instance.GetCharacter(j);

                if (target == moveCharacter) continue;
                if (target.position == nextPosition) targetCharacterList.Add(target);
            }

            // 停止マスでなければ次へ
            if (!stageManager.CheckStopSquare(moveCharacter.position)) continue;

            await ExcuteSquareEvent(moveCharacter);
        }
    }

    /// <summary>
    /// マスのイベント実行
    /// </summary>
    /// <param name="target"></param>
    private async UniTask ExcuteSquareEvent(Character target)
    {
        int squareEventID = StageManager.instance.GetSquareEvent(target.position);
        await EventManager.ExecuteEvent(target, squareEventID);
    }

    /// <summary>
    /// カード使用
    /// </summary>
    /// <param name="index"></param>
    public void AcceptCard(int index)
    {
        _acceptEnd = true;
        _handIndex = index;
    }
}
