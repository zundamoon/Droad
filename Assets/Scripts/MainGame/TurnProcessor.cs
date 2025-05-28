using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

using static CommonModule;
using static GameConst;
using UnityEngine.TextCore.Text;

public class TurnProcessor
{
    public static List<int> playerOrder = null;

    private const int _STAR_EVENT_ID = 12;
    private const int _TURN_ANNOUNCE_ID = 101;
    private const int _ORDER_TURN_ANNOUNCE_ID = 102;
    private const int _PLAY_ANNOUNCE_ID = 103;

    public void Init()
    {
        playerOrder = new List<int>(PLAYER_MAX);
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            playerOrder.Add(i);
        }
        UIManager.instance.AddStatus(playerOrder);
        CharacterManager.instance.UpdateRank();
    }

    /// <summary>
    /// 各ターンの処理
    /// </summary>
    public async UniTask TurnProc()
    {
        // カメラをステージに向ける
        await CameraManager.SetAnchor(StageManager.instance.GetCameraAnchor());
        // ドロー
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(i);
            await character.possessCard.DrawDeckMax();
        }

        // 手番決め
        await UIManager.instance.RunMessage(_ORDER_TURN_ANNOUNCE_ID.ToText());
        await DesidePlayerOrder();
        await UIManager.instance.ScrollAllStatus();
        await UIManager.instance.AddStatus(playerOrder);
        await UIManager.instance.AddStatus(_ORDER_TURN_ANNOUNCE_ID.ToText());

        // 各手番
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            int orderIndex = playerOrder[i];
            Character character = CharacterManager.instance.GetCharacter(orderIndex);
            if (character == null) return;

            // UI表示
            await CameraManager.SetAnchor(character.GetCameraAnchor());
            await UIManager.instance.ReSizeTop();
            await EachTurn(character, orderIndex);
            await UIManager.instance.ScrollStatus();
            await UIManager.instance.AddStatus(playerOrder[i]);
        }
    }

    /// <summary>
    /// 手番を決める
    /// </summary>
    private async UniTask DesidePlayerOrder()
    {
        List<int> playCardList = new List<int>(PLAYER_MAX);
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(playerOrder[i]);
            // 手札の選択
            // UIの表示
            await UIManager.instance.ReSizeTop();
            await UIManager.instance.RunMessage(string.Format(_PLAY_ANNOUNCE_ID.ToText()));
            int handIndex = -1;
            await UIManager.instance.SetOnUseCard((index) =>
            {
                handIndex = index;
            });
            await UIManager.instance.OpenHandArea(character.possessCard);
            int playCardCount = GetOrderCount(handIndex, character);
            playCardList.Add(playCardCount);
            await UIManager.instance.ScrollStatus();
            await UIManager.instance.AddStatus(playerOrder[i]);
        }
        playerOrder.Clear();
        // 出されたカードから順番を決める
        while (playerOrder.Count < PLAYER_MAX)
        {
            int maxValue = playCardList.Max();
            List<int> indexList = new();
            for (int i = 0; i < playCardList.Count; i++)
            {
                if (playCardList[i] == maxValue)
                {
                    indexList.Add(i);
                }
            }
            // indexList からランダムな順番で playerOrder に追加
            while (indexList.Count > 0)
            {
                int rand = Random.Range(0, indexList.Count);
                int chosenIndex = indexList[rand];
                playerOrder.Add(chosenIndex);
                playCardList[chosenIndex] = -1;
                indexList.RemoveAt(rand);
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
        possess.DiscardHandIndex(handIndex);
        return CardManager.GetCard(ID).advance;
    }

    /// <summary>
    /// キャラクターのターン処理
    /// </summary>
    /// <param name="turnCharacter"></param>
    private async UniTask EachTurn(Character turnCharacter, int order)
    {
        // 手札がないならスキップ
        if (turnCharacter.possessCard.handCardIDList.Count <= 0) return;

        await UIManager.instance.RunMessage(string.Format(_TURN_ANNOUNCE_ID.ToText(), order + 1));
        int handIndex = -1;
        await UIManager.instance.SetOnUseCard((index) =>
        {
            handIndex = index;
        });

        // 非同期で処理を開始
        var task = UIManager.instance.OpenHandArea(turnCharacter.possessCard);

        // 終了まで毎フレームカメラ処理
        while (!task.Status.IsCompleted())
        {
            CameraManager.instance.CameraDrag();
            CameraManager.instance.CameraZoom();
            await UniTask.Yield();
        }

        // カメラの位置をプレイヤーの元に戻す
        await CameraManager.SetAnchor(turnCharacter.GetCameraAnchor());

        // カードの使用
        int advanceValue = await turnCharacter.possessCard.UseCard(handIndex, turnCharacter);
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
    /// キャラクターの移動
    /// </summary>
    /// <param name="advanceValue"></param>
    /// <param name="moveCharacter"></param>
    /// <param name="targetCharacterList"></param>
    /// <returns></returns>
    private async UniTask MoveCharacter(int advanceValue, Character moveCharacter, List<Character> targetCharacterList)
    {
        StageManager stageManager = StageManager.instance;
        int playerID = moveCharacter.playerID;
        // 進む分だけ動く
        for (int i = 0; i < advanceValue; i++)
        {
            // 移動先のマスを取得
            stageManager.GetSquare(moveCharacter.position).DeleteStandingList(playerID);
            // StagePosition nextPosition = stageManager.GetNextPosition(moveCharacter.position);
            StagePosition nextPosition = moveCharacter.nextPosition;
            stageManager.GetSquare(nextPosition).AddStandingList(playerID);
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

            await UniTask.DelayFrame(15);

            var nextPositionList = stageManager.GetSquare(moveCharacter.position).GetNextPosition();
            if (nextPositionList.Count == 1) moveCharacter.nextPosition = nextPositionList[0];

            // 最後のマスでないか停止マスでなければ次へ
            if (i >= advanceValue - 1 || !stageManager.CheckStopSquare(moveCharacter.position)) continue;

            await ExcuteSquareEvent(moveCharacter);
        }
    }

    /// <summary>
    /// マスのイベント実行
    /// </summary>
    /// <param name="target"></param>
    private async UniTask ExcuteSquareEvent(Character target)
    {
        Square targetSquare = StageManager.instance.GetSquare(target.position);
        int eventID = targetSquare.GetEventID();

        // イベントの繰り返し
        for (int i = 0, max = target.eventRepeatCount; i < max; i++)
        {
            // スターマスならスターイベントを実行
            if (targetSquare.GetIsStarSquare()) eventID = _STAR_EVENT_ID;

            EventContext context = new EventContext()
            {
                character = target,
                square = targetSquare,
            };

            await EventManager.ExecuteEvent(eventID, context);

            // 複数実行可マス出ないなら終わる
            if (!targetSquare.GetSquareData().canRepeatSquare) break;
        }
        target.SetRepeatEventCount(1);
    }
}
