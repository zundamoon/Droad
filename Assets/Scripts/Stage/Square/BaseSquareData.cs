using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonModule;

public abstract class BaseSquareData
{
    // �X�e�[�W��̃}�X�̈ʒu
    public StagePosition squarePosition;
    // ���Ɉړ��ł���}�X�̌��
    public List<StagePosition> nextPositionList;
    // �}�X�̐F
    public Color squareColor { get; protected set; }
    // �C�x���gID
    public int eventID { get; protected set; }

    public bool isStarSquare;
    public bool isStopSquare { get; protected set; }
    public bool canRepeatSquare { get; protected set; }


    public BaseSquareData()
    {
        squareColor = Color.white;
        eventID = -1;
    }

    public void ChangeColor(Color color) { squareColor = color; }
}
