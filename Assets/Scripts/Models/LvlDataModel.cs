using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public enum LvlStatus { Close, Open, Finished }


public class LvlDataModel
{

    public ReactiveProperty<int> currentLvl { get; set; }
    public ReactiveProperty<int> currentAsteroid { get; set; }
    public ReactiveProperty<int> maxCurAster { get; set; }

    public int allCountLvl = 3;
    public int defCurLvl = 1;

    public LvlStruct[] lvlStruct;   

    public LvlDataModel()
    {
        this.currentLvl = new ReactiveProperty<int>(defCurLvl);
        this.currentAsteroid = new ReactiveProperty<int>();
        this.maxCurAster = new ReactiveProperty<int>();

        lvlStruct = new LvlStruct[allCountLvl];
        

        //просто для удобства заполнения xml файла
        for(int i=0;i<allCountLvl; i++) 
        {
            lvlStruct[i].numberLvl = i;
            
            
        }
    }

    public void SetCurrentLvl(int x)
    {
        currentLvl.Value =x;
    }
    public int GetCurrentLvl()
    {
        return currentLvl.Value;
    }
    public ref LvlStruct GetLvlStruct()
    {
        return ref lvlStruct[currentLvl.Value - 1];
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
