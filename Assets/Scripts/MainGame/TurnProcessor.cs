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
    private bool _acceptEnd = false;
    private int _handIndex = -1;

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
    /// �e�^�[���̏���
    /// </summary>
    public async UniTask TurnProc()
    {
        // �J�������X�e�[�W�Ɍ�����
        await CameraManager.SetAnchor(StageManager.instance.GetCameraAnchor());
        // �h���[
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(i);
            await character.possessCard.DrawDeckMax();
        }

        // ��Ԍ���
        await UIManager.instance.RunMessage(_ORDER_TURN_ANNOUNCE_ID.ToText());
        await DesidePlayerOrder();
        await UIManager.instance.ScrollAllStatus();
        await UIManager.instance.AddStatus(playerOrder);
        await UIManager.instance.AddStatus(_ORDER_TURN_ANNOUNCE_ID.ToText());

        // �e���
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            int orderIndex = playerOrder[i];
            Character character = CharacterManager.instance.GetCharacter(orderIndex);
            if (character == null) return;

            // UI�\��
            await CameraManager.SetAnchor(character.GetCameraAnchor());
            await UIManager.instance.OpenHandArea(character.possessCard);
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
        List<int> playCardList = new List<int>(PLAYER_MAX);
        for (int i = 0; i < PLAYER_MAX; i++)
        {
            Character character = CharacterManager.instance.GetCharacter(playerOrder[i]);
            // ��D�̑I��
            // UI�̕\��
            await UIManager.instance.OpenHandArea(character.possessCard);
            await UIManager.instance.ReSizeTop();
            UIManager.instance.StartHandAccept();
            await UIManager.instance.RunMessage(string.Format(_PLAY_ANNOUNCE_ID.ToText()));
            while (!_acceptEnd)
            {
                await UniTask.DelayFrame(1);
            }
            _acceptEnd = false;
            int playCardCount = GetOrderCount(_handIndex, character);
            playCardList.Add(playCardCount);
            await UIManager.instance.ScrollStatus();
            await UIManager.instance.AddStatus(playerOrder[i]);
        }
        playerOrder.Clear();
        // �o���ꂽ�J�[�h���珇�Ԃ����߂�
        while (playerOrder.Count < PLAYER_MAX)
        {
            // �ő�l�̃C���f�b�N�X���擾
            int maxValue = playCardList.Max();
            int playCount = playCardList.Count;
            List<int> indexList = new List<int>(playCount);
            for (int i = 0; i < playCardList.Count; i++)
            {
                if (playCardList[i] != maxValue) continue;
                indexList.Add(i);
                // �����Ȃ烉���_���Ɍ���
                while (indexList.Count > 0)
                {
                    int index = Random.Range(0, indexList.Count);
                    playerOrder.Add(indexList[index]);
                    indexList.RemoveAt(index);
                }
                playCardList[i] = -1;
            }
        }
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
        return CardManager.GetCard(ID).advance;
    }

    /// <summary>
    /// �L�����N�^�[�̃^�[������
    /// </summary>
    /// <param name="turnCharacter"></param>
    private async UniTask EachTurn(Character turnCharacter, int order)
    {
        // ��D���Ȃ��Ȃ�X�L�b�v
        if (turnCharacter.possessCard.handCardIDList.Count <= 0) return;

        UIManager.instance.StartHandAccept();
        await UIManager.instance.RunMessage(string.Format(_TURN_ANNOUNCE_ID.ToText(), order + 1));
        // ��D���g����܂őҋ@
        while (!_acceptEnd)
        {
            CameraManager.instance.CameraDrag();
            CameraManager.instance.CameraZoom();
            await UniTask.DelayFrame(1);
        }

        // �J�����̈ʒu��߂�
        await CameraManager.SetAnchor(turnCharacter.GetCameraAnchor());

        _acceptEnd = false;

        // �J�[�h�̎g�p
        int advanceValue = await turnCharacter.possessCard.UseCard(_handIndex, turnCharacter);
        if (advanceValue <= 0) return;

        // �L�����N�^�[�𓮂���
        List<Character> targetCharacterList = new List<Character>(PLAYER_MAX);
        await MoveCharacter(advanceValue, turnCharacter, targetCharacterList);

        // �ړ��㏈�������s
        turnCharacter.ExecuteAfterMoveEvent(targetCharacterList);

        // �C�x���g�\�łȂ���ΏI��
        if (!turnCharacter.CanEvent()) return;
        await ExcuteSquareEvent(turnCharacter);
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
            for (int j = 0; j < PLAYER_MAX; j++)
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

            EventContext context = new EventContext()
            {
                character = target,
                square = targetSquare,
            };

            await EventManager.ExecuteEvent(eventID, context);

            // �������s�}�X�o�Ȃ��Ȃ�I���
            if (!targetSquare.GetSquareData().canRepeatSquare) break;
        }
        target.SetRepeatEventCount(1);
    }

    /// <summary>
    /// �J�[�h�g�p
    /// </summary>
    /// <param name="index"></param>
    public void AcceptCard(int index)
    {
        _acceptEnd = true;
        _handIndex = index;
    }
}
