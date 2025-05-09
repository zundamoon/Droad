using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TurnProcessor
{
    private List<Character> _playerList = null;
    private List<int> _playerOrder = null;

    private const int _PLAYER_MAX = 4;

    public void Init()
    {
        _playerList = new List<Character>(_PLAYER_MAX);
        _playerOrder = new List<int>(_PLAYER_MAX);
    }

    /// <summary>
    /// �e�^�[���̏���
    /// </summary>
    public async UniTask TurnProc()
    {
        // ��Ԍ���
        await DesidePlayerOrder();

        // �e���
        for (int i = 0; i < _PLAYER_MAX; i++)
        {
            int orderIndex = _playerOrder[i];
            await EachTurn(_playerList[orderIndex]);
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
        // ��D��I��
        int cardID = -1;
        // �J�[�h���擾
        CardData useCard = CardManager.GetCard(cardID);
        if (useCard == null) return;
        // �R�C���𑝂₷
        turnCharacter.AddCoin(useCard.addCoin);
        // �J�[�h��ID����i�މ񐔂��擾
        int advanceValue = useCard.advance;
        // �i�ޕ���������
        // �����r���̃L�����N�^�[��ێ�
        List<Character> targetCharacterList = new List<Character>(_PLAYER_MAX);
        for (int i = 0; i < advanceValue; i++)
        {
            // �ړ�
            // character.Move();
            // ���݂̃}�X���擾
            // �}�X����L�����N�^�[���擾
            //targetCharacter.Add();
            // ���݂̃}�X����~�}�X������
            if (false) continue;

            // �L�����N�^�[�̃}�X����C�x���g���擾�����s
            if (!turnCharacter.CanEvent()) return;

        }
        // �ړ��㏈��
        turnCharacter.ExecuteAfterMoveEvent(targetCharacterList);

        // �L�����N�^�[�̃}�X����C�x���g���擾�����s
        if (!turnCharacter.CanEvent()) return;
        
    }
}
