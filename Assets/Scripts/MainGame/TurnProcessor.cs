using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

using static CommonModule;
using static GameConst;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class TurnProcessor
{
    public static List<int> playerOrder = null;

    private const int _STAR_EVENT_ID = 12;
    private const int _TURN_ANNOUNCE_ID = 101;
    private const int _ORDER_TURN_ANNOUNCE_ID = 102;
    private const int _PLAY_ANNOUNCE_ID = 103;

    public void Init()
    {
        playerOrder = new List<int>(GameDataManager.instance.playerMax);
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            playerOrder.Add(i);
        }
        UIManager.instance.AddStatus(playerOrder);
        CharacterManager.instance.UpdateRank();
    }

    /// <summary>
    /// �e�^�[���̏���
    /// </summary>
    public async UniTask TurnProc()
    {
        // �J�������X�e�[�W�Ɍ�����
        await CameraManager.SetAnchor(StageManager.instance.GetCameraAnchor());
        // �h���[
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(i);
            await character.possessCard.DrawDeckMax();
        }

        // ��Ԍ���
        //await UIManager.instance.RunMessage(_ORDER_TURN_ANNOUNCE_ID.ToText());
        //await DesidePlayerOrder();
        //await UIManager.instance.ScrollAllStatus();
        //await UIManager.instance.AddStatus(playerOrder);
        //await UIManager.instance.AddStatus(_ORDER_TURN_ANNOUNCE_ID.ToText());

        // �e���
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            int orderIndex = playerOrder[i];
            Character character = CharacterManager.instance.GetCharacter(orderIndex);
            if (character == null) return;

            // UI�\��
            PossessCard possessCard = character.possessCard;
            UIManager.instance.SetPossessCard(ref possessCard);
            await UIManager.instance.CloseDetail();
            await CameraManager.SetAnchor(character.GetCameraAnchor());
            await UIManager.instance.ReSizeTop();
            await EachTurn(character, orderIndex);
            await UIManager.instance.ScrollStatus();
            await UIManager.instance.AddStatus(playerOrder[i]);
        }
    }

    /// <summary>
    /// ��Ԃ����߂�
    /// </summary>
    private async UniTask DesidePlayerOrder()
    {
        Dictionary<int, int> playCardMap = new Dictionary<int, int>();
        for (int i = 0; i < GameDataManager.instance.playerMax; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(playerOrder[i]);
            // ��D�̑I��
            // UI�̕\��
            PossessCard possessCard = character.possessCard;
            UIManager.instance.SetPossessCard(ref possessCard);
            await UIManager.instance.CloseDetail();
            await UIManager.instance.ReSizeTop();
            await UIManager.instance.RunMessage(string.Format(_PLAY_ANNOUNCE_ID.ToText()));
            int handIndex = -1;
            await UIManager.instance.SetOnUseCard((index) =>
            {
                handIndex = index;
            });

            var task = UIManager.instance.OpenHandArea(character.possessCard);

            // �I���܂Ŗ��t���[���J��������
            while (!task.Status.IsCompleted())
            {
                await UniTask.DelayFrame(1);
                if (!UIManager.instance.IsHandAccept) continue;
                CameraManager.instance.CameraDrag();
                CameraManager.instance.CameraZoom();
            }
            // �J�����̈ʒu���v���C���[�̌��ɖ߂�
            await CameraManager.SetAnchor(StageManager.instance.GetCameraAnchor());

            int playerID = playerOrder[i];
            int playCardCount = GetOrderCount(handIndex, character);
            playCardMap[playerID] = playCardCount;
            await UIManager.instance.CloseDetail();
            await UIManager.instance.ScrollStatus();
            await UIManager.instance.AddStatus(playerID);
        }
        playerOrder.Clear();
        // �o���ꂽ�J�[�h���珇�Ԃ����߂�
        var sorted = playCardMap
        .GroupBy(kvp => kvp.Value)
        .OrderByDescending(g => g.Key)
        .SelectMany(g =>
        {
            List<int> ids = g.Select(kvp => kvp.Key).ToList();
            // ���_�̒��Ń����_���ɕ��ёւ�
            for (int i = 0; i < ids.Count; i++)
            {
                int j = UnityEngine.Random.Range(i, ids.Count);
                (ids[i], ids[j]) = (ids[j], ids[i]);
            }
            return ids;
        })
        .ToList();

        playerOrder = sorted;
    }

    /// <summary>
    /// ��D�ԍ����w�肵���Ԃ̐������擾
    /// </summary>
    /// <param name="handIndex"></param>
    /// <returns></returns>
    private int GetOrderCount(int handIndex, Character playCharacter)
    {
        PossessCard possess = playCharacter.possessCard;
        int ID = possess.handCardIDList[handIndex];
        possess.DiscardHandIndex(handIndex);
        return CardManager.instance.GetCard(ID).advance;
    }

    /// <summary>
    /// �L�����N�^�[�̃^�[������
    /// </summary>
    /// <param name="turnCharacter"></param>
    private async UniTask EachTurn(Character turnCharacter, int order)
    {
        await turnCharacter.SquareStand();

        // ��D���Ȃ��Ȃ�X�L�b�v
        if (turnCharacter.possessCard.handCardIDList.Count <= 0) return;

        await UIManager.instance.RunMessage(string.Format(_TURN_ANNOUNCE_ID.ToText(), order + 1));
        int handIndex = -1;
        await UIManager.instance.SetOnUseCard((index) =>
        {
            handIndex = index;
        });

        // �񓯊��ŏ������J�n
        var task = UIManager.instance.OpenHandArea(turnCharacter.possessCard);

        // �I���܂Ŗ��t���[���J��������
        while (!task.Status.IsCompleted())
        {
            await UniTask.DelayFrame(1);
            if (!UIManager.instance.IsHandAccept) continue;
            CameraManager.instance.CameraDrag();
            CameraManager.instance.CameraZoom();
        }

        // �J�����̈ʒu���v���C���[�̌��ɖ߂�
        await CameraManager.SetAnchor(turnCharacter.GetCameraAnchor());

        // �J�[�h�̎g�p
        int advanceValue = await turnCharacter.possessCard.UseCard(handIndex, turnCharacter);
        if (advanceValue <= 0) return;
        await UIManager.instance.CloseDetail();
        // �L�����N�^�[�𓮂���
        List<Character> targetCharacterList = new List<Character>(GameDataManager.instance.playerMax);
        await MoveCharacter(advanceValue, turnCharacter, targetCharacterList);

        // �ړ��㏈�������s
        turnCharacter.ExecuteAfterMoveEvent(targetCharacterList);

        // �C�x���g�\�łȂ���ΏI��
        if (!turnCharacter.CanEvent()) return;
        await ExcuteSquareEvent(turnCharacter);

        // �ړ���ɃC�x���g���s�������Z�b�g
        turnCharacter.SetRepeatEventCount(1);
        await turnCharacter.SquareShift();
    }

    /// <summary>
    /// �L�����N�^�[�̈ړ�
    /// </summary>
    /// <param name="advanceValue"></param>
    /// <param name="moveCharacter"></param>
    /// <param name="targetCharacterList"></param>
    /// <returns></returns>
    private async UniTask MoveCharacter(int advanceValue, Character moveCharacter, List<Character> targetCharacterList)
    {
        StageManager stageManager = StageManager.instance;
        int playerID = moveCharacter.playerID;
        // �i�ޕ���������
        for (int i = 0; i < advanceValue; i++)
        {
            // �ړ���̃}�X���擾
            stageManager.GetSquare(moveCharacter.position).DeleteStandingList(playerID);
            // StagePosition nextPosition = stageManager.GetNextPosition(moveCharacter.position);
            StagePosition nextPosition = moveCharacter.nextPosition;
            stageManager.GetSquare(nextPosition).AddStandingList(playerID);
            Vector3 movePosition = stageManager.GetPosition(nextPosition);

            // �ړ�
            await moveCharacter.Move(movePosition);
            moveCharacter.position = nextPosition;
            // �}�X��ɂ��鑼�L���������o
            for (int j = 0; j < GameDataManager.instance.playerMax; j++)
            {
                Character target = CharacterManager.instance.GetCharacter(j);

                if (target == moveCharacter) continue;
                if (target.position == nextPosition) targetCharacterList.Add(target);
            }

            await UniTask.DelayFrame(15);

            var nextPositionList = stageManager.GetSquare(moveCharacter.position).GetNextPosition();
            if (nextPositionList.Count == 1) moveCharacter.nextPosition = nextPositionList[0];

            // �Ō�̃}�X�łȂ�����~�}�X�łȂ���Ύ���
            if (i >= advanceValue - 1 || !stageManager.CheckStopSquare(moveCharacter.position)) continue;

            await ExcuteSquareEvent(moveCharacter);
        }
    }

    /// <summary>
    /// �}�X�̃C�x���g���s
    /// </summary>
    /// <param name="target"></param>
    private async UniTask ExcuteSquareEvent(Character target)
    {
        Square targetSquare = StageManager.instance.GetSquare(target.position);
        int eventID = targetSquare.GetEventID();

        // �C�x���g�̌J��Ԃ�
        for (int i = 0, max = target.eventRepeatCount; i < max; i++)
        {
            // �X�^�[�}�X�Ȃ�X�^�[�C�x���g�����s
            if (targetSquare.GetIsStarSquare()) eventID = _STAR_EVENT_ID;
            else eventID = targetSquare.GetEventID();

            EventContext context = new EventContext()
                {
                    character = target,
                    square = targetSquare,
                };

            await EventManager.ExecuteEvent(eventID, context);

            // �������s�}�X�o�Ȃ��Ȃ�I���
            if (!targetSquare.GetSquareData().canRepeatSquare) break;
        }
    }
}
