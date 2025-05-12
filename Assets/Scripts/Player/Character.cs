using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

using static CommonModule;

public class Character : MonoBehaviour
{
    // ���g
    public GameObject playerObject = null;
    // �����J�[�h
    public PossessCard possessCard { get; private set; } = null;
    // �R�C��
    public int coins { get; private set; } = -1;
    // �X�^�[
    public int stars { get; private set; } = -1;
    // ���ʖ����t���O
    public bool eventCancel { get; private set; } = false;
    // �ړ���̃C�x���g
    public Action<List<Character>> AfterMoveEvent { get; private set; } = null;

    // �ʒu���
    public StagePosition position;
    // ���̈ړ����ێ�
    public StagePosition nextPosition;

    public float moveSpeed = 3.0f;

    public float goalDistance = 0.05f;

    public void Init()
    {
        playerObject = GetComponent<GameObject>();
        possessCard = new PossessCard();
        possessCard.Init();
        position.route = 0;
        position.road = 0;
        position.square = 0;
    }
    /// <summary>
    /// �ړ���C�x���g�̐ݒ�
    /// </summary>
    /// <param name="setEvent"></param>
    public void SetAfterMoveEvent(Action<List<Character>> setEvent) { AfterMoveEvent = setEvent; }
    public void ExecuteAfterMoveEvent(List<Character> targetCharacterList)
    {
        if (AfterMoveEvent == null) return;
        AfterMoveEvent(targetCharacterList);
        AfterMoveEvent = null;
    }

    public void SetCoin(int value) { coins = value; }
    public void AddCoin(int value) { coins += value; }
    public int RemoveCoin(int value)
    {
        int removeCoin = Math.Max(0, coins - value);
        coins -= removeCoin;
        return removeCoin;
    }
    public void AddStar(int value) { stars += value; }
    public int RemoveStar(int value)
    {
        int removeStar = Math.Max(0, stars - value);
        stars -= removeStar;
        return removeStar;
    }
    public void SetCancelEvent() { eventCancel = true; }
    /// <summary>
    /// �C�x���g�����s�ł��邩
    /// </summary>
    /// <returns></returns>
    public bool CanEvent()
    {
        if (eventCancel)
        {
            eventCancel = false;
            return true;
        }
        return false;
    }
    /// <summary>
    /// �ړ��֐�
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public async UniTask Move(Vector3 targetPos)
    {
        // �ړ����[�v
        while (Vector3.Distance(playerObject.transform.position, targetPos) > goalDistance)
        {
            // ��ԂŊ��炩�Ɉړ�
            transform.position = Vector3.Lerp(playerObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
            await UniTask.DelayFrame(1);
        }
        transform.position = targetPos;
    }
}
