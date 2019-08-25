using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BulletModel 
{
    public int Speed { get; set; }
    public ReactiveProperty<Vector3> Position { get; set; }

    public BulletModel(Vector3 position, int speed)
    {
        
        this.Speed = speed;
        this.Position = new ReactiveProperty<Vector3>(position);

        //this.IsDead = this.CurrentHp.Select(x => x <= 0).ToReactiveProperty();
    }

    public void moveObject()
    {
        Observable.EveryFixedUpdate().Subscribe(_ =>
        Position.Value = new Vector3(Position.Value.x, Position.Value.y + Time.fixedDeltaTime * Speed, Position.Value.z));

    }


    
}
