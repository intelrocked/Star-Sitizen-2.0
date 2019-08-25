using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerSettingsModel
{
    public float playerSpeed = 10;
    public float playerExp = 0;
    public int playerHP = 3;
    public ReactiveProperty<int> CurrentHp { get; set; }
    public int bulletSpeed = 3;

    public PlayerSettingsModel()
    {
        this.CurrentHp = new ReactiveProperty<int>(playerHP);
    }

}
