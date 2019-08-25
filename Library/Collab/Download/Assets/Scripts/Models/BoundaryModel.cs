using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BoundaryModel{

    public Vector2 min { get; set; } = new Vector2();
    public Vector2 max { get; set; } = new Vector2();

    public void SetBoundary(Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
    }

}
