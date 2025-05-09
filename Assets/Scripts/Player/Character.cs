using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // �����J�[�h
    public PossessCard possessCard { get; private set; } = null;
    // �R�C��
    public int coins { get; private set; } = -1;
    // �X�^�[
    public int stars { get; private set; } = -1;
    // ���ʖ����t���O
    public bool eventCancel { get; private set; } = false;
    // �ړ���̃C�x���g
    private Action<List<Character>> _AfterMoveEvent = null;

    public void Init()
    {
        possessCard = new PossessCard();
        possessCard.Init();
    }
    /// <summary>
    /// �ړ���C�x���g�̐ݒ�
    /// </summary>
    /// <param name="setEvent"></param>
    public void SetAfterMoveEvent(Action<List<Character>> setEvent) { _AfterMoveEvent = setEvent; }
    public void ExecuteAfterMoveEvent(List<Character> targetCharacterList)
    {
        if (_AfterMoveEvent == null) return;
        _AfterMoveEvent(targetCharacterList);
        _AfterMoveEvent = null;
    }

    public void SetCoin(int value) { coins = value; }
    public void AddCoin(int value) { coins += value; }
    public int RemoveCoin(int value)
    {
        int removeCoin = Math.Max(0, coins - value);
        coins -= removeCoin;
        return removeCoin;
    }
    public void AddStar(int value) {  stars += value; }
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
}
