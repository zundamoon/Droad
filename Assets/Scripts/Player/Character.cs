using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int coins { get; private set; }

    public int GetCoin() { return coins; }
    public void SetCoin(int value) { coins = value; }
    public void AddCoin(int value) { coins += value; }
    public void RemoveCoin(int value) { Math.Max(0, coins - value); }
}
