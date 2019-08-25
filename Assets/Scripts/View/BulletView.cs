using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BulletView : GameElement
{

    BulletModel bulletModel;
    GameController gameController;

    private CompositeDisposable disposables = new CompositeDisposable();

    void Start()
    {
        gameController = app.controller;
    }

    public void setModel(BulletModel model)
    {
        bulletModel = model;        
        bulletModel.Position
             .ObserveEveryValueChanged(x => x.Value)
             .Subscribe(xs =>
             {
                 transform.position = xs;
             }).AddTo(disposables);

    }

    public void OnTriggerEnter(Collider other)
    {
        gameController.TriggerBulletDeath(other, bulletModel, gameObject);
    }

    public void OnDestroy()
    {
       
        disposables.Dispose();

    }
}
