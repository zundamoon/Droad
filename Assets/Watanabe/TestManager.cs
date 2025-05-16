using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using static CommonModule;
using System.Threading.Tasks;

public class TestManager : MonoBehaviour
{
    [SerializeField] public List<Character> playerObjectList = null;
    private List<Character> _playerList = null;
    private List<int> _playerOrder = null;

    private const int _PLAYER_MAX = 1;

    private void Awake()
    {
        MasterDataManager.LoadAllData();
        EventManager.Init();
        _playerList = new List<Character>(_PLAYER_MAX);
        _playerOrder = new List<int>(_PLAYER_MAX);

        for (int i = 0; i < _PLAYER_MAX; i++)
        {
            _playerList.Add(playerObjectList[i]);
        }
    }

    private void Start()
    {
        TurnProc();
    }

    /// <summary>
    /// �e�^�[���̏���
    /// </summary>
    public async UniTask TurnProc()
    {
        while (true)
        {
            // �e���
            for (int i = 0; i < _playerList.Count; i++)
            {
                await EachTurn(_playerList[i]);
                Debug.Log(_playerList[i].coins);
            }
        }
    }

    /// <summary>
    /// ��Ԃ����߂�
    /// </summary>
    private async UniTask DesidePlayerOrder()
    {
        _playerOrder.Clear();
        List<int> playCardList = new List<int>(_PLAYER_MAX);
        for (int i = 0; i < _PLAYER_MAX; i++)
        {
            // ��D�̑I��
            int playCardCount = 0;
            playCardList.Add(playCardCount);
        }
        // �o���ꂽ�J�[�h���珇�Ԃ����߂�
        while (playCardList.Count > 0)
        {
            // �ő�l�̃C���f�b�N�X���擾
            int max = playCardList.Max();
            List<int> indexList = new List<int>();
            for (int j = 0; j < _PLAYER_MAX; j++)
            {
                if (indexList[j] != max) continue;
                indexList.Add(j);
                playCardList.RemoveAt(j);
            }
            // �����Ȃ烉���_���Ɍ���
            while (indexList.Count > 0)
            {
                int index = Random.Range(0, indexList.Count);
                _playerOrder.Add(indexList[index]);
                indexList.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// �L�����N�^�[�̃^�[������
    /// </summary>
    /// <param name="turnCharacter"></param>
    private async UniTask EachTurn(Character turnCharacter)
    {
        //if(turnCharacter == null) return;

        //StageManager stageManager = StageManager.instance;
        //// �J�[�h��ID����i�މ񐔂��擾
        //int advanceValue = 2;
        //// �i�ޕ���������
        //// �����r���̃L�����N�^�[��ێ�
        //List<Character> targetCharacterList = new List<Character>(_PLAYER_MAX);
        //for (int i = 0; i < advanceValue; i++)
        //{
        //    // �ړ���̃}�X���擾
        //    StagePosition nextPosition = stageManager.CheckNextPosition(turnCharacter.position);
        //    Vector3 movePosition = stageManager.GetPosition(nextPosition);

        //    // �ړ�
        //    await turnCharacter.Move(movePosition);
        //    turnCharacter.position = nextPosition;
        //    // �}�X��ɂ��鑼�L���������o
        //    for (int j = 0; j < _PLAYER_MAX; j++)
        //    {
        //        Character target = _playerList[j];

        //        if (target == turnCharacter) continue;
        //        if (target.position == nextPosition) targetCharacterList.Add(target);
        //    }

        //    // ��~�}�X�łȂ���Ύ���
        //    if (!stageManager.CheckStopSquare(turnCharacter.position)) continue;
        //    // �C�x���g�\�Ȃ炻�̏�ŏI��
        //    if (!turnCharacter.CanEvent()) return;

        //    ExcuteSquareEvent(turnCharacter);
        //}

        //await UniTask.DelayFrame(1000);

        //// �ړ��㏈�������s
        //turnCharacter.ExecuteAfterMoveEvent(targetCharacterList);

        //// �C�x���g�\�łȂ���ΏI��
        //if (!turnCharacter.CanEvent()) return;
        //ExcuteSquareEvent(turnCharacter);
    }

    private void ExcuteSquareEvent(Character target)
    {
        int squareEventID = StageManager.instance.GetSquareEvent(target.position);
        EventManager.ExecuteEvent(target, squareEventID);
    }
}


