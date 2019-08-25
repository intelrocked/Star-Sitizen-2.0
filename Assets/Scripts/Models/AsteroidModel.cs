using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class AsteroidModel 
{
    public ReactiveProperty<int> CurrentHp { get; set; }
    public float Speed { get; set; }
    public ReactiveProperty<Vector3> Position { get; set; }
    

    public AsteroidModel(int initialHp, Vector3 position, float speed)
    {
        this.CurrentHp = new ReactiveProperty<int>(initialHp);
        this.Position = new ReactiveProperty<Vector3>(position);
        this.Speed = speed;
    }

    public void moveObject()
    {
        Observable.EveryFixedUpdate().Subscribe(_ => 
        Position.Value = new Vector3(Position.Value.x, Position.Value.y - Time.fixedDeltaTime * Speed, Position.Value.z));
        
    }

}
