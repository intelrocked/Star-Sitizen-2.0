using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class AsteroidView : GameElement
{

    AsteroidModel asteroidModel;
    GameController gameController;

    private CompositeDisposable disposables = new CompositeDisposable();

    void Start()
    {
        gameController = app.controller;
    }

    public void setModel(AsteroidModel model)
    {
        asteroidModel = model;
        
        asteroidModel.Position
             .ObserveEveryValueChanged(x => x.Value)
             .Subscribe(xs =>
             {
                 transform.position = xs;
             }).AddTo(disposables);
             
    }

    public void OnTriggerEnter(Collider other)
    {
        gameController.TriggerDeathZ(other,  asteroidModel, gameObject);
    }

    public void OnDestroy()
    {
        
        disposables.Dispose();
       
    }

 
}
