using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public enum LvlStatus { Close, Open, Finished }


public class LvlDataModel
{

    public ReactiveProperty<int> currentLvl { get; set; }
    public int allCountLvl = 3;

    public LvlStruct[] lvlStruct;

    public LvlDataModel()
    {
        lvlStruct = new LvlStruct[allCountLvl];
        this.currentLvl = new ReactiveProperty<int>(1);

        //просто для удобства заполнения xml файла
        for(int i=0;i<allCountLvl; i++) 
        {
            lvlStruct[i].numberLvl = i;
            
            
        }
    }
}

public struct LvlStruct
{
    public int numberLvl;
    public LvlStatus status; 
    public int asteroidsCount;
    public int asteroidsHP;
    public Vector3 asteroidPos;
    public float CoroutineRate;

    public float asteroidsSpeed;
}
