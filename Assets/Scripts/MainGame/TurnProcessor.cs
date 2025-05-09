using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnProcessor
{
    private List<Character> _playerList = null;

    private const int _PLAYER_MAX = 4;

    public void Init()
    {
        _playerList = new List<Character>(_PLAYER_MAX);
    }

    public void TurnProc()
    {
        // ��Ԍ���
        DesidePlayerOrder();

        // �e���
        for (int i = 0; i < _PLAYER_MAX; i++)
        {
            EachTurn(_playerList[i]);
        }
    }

    /// <summary>
    /// ��Ԃ����߂�
    /// </summary>
    private void DesidePlayerOrder()
    {
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
                _playerList.Add(indexList[index]);
                indexList.RemoveAt(index);
            }
        }
    }

    private void EachTurn(Character turnCharacter)
    {
        // ��D��I��


        // �ړ�����Ώ�
        Character character = turnCharacter;
        // �J�[�h��ID����i�މ񐔂��擾
        int advanceValue = 6;
        // �i�ޕ���������
        for (int i = 0; i < advanceValue; i++)
        {
            // �i�s���̓��Ɏ��̃}�X�����邩�m�F
            if (StageManager.instance.CheckNextSqaure(character.position) == null)
            {
                // ���򂪂���Γ���I���i�I�������܂œ����Ȃ��j

            }
            // �ړ�
            // character.Move();
        }
    }
}
