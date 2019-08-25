using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;



public class PlayerView : GameElement
{

    GameController gameController;
    PlayerSettingsModel playerModel;

    private CompositeDisposable disposables = new CompositeDisposable();

    void Start()
    {
        gameController = app.controller;
        playerModel = app.model.playerSettings;

        playerModel.CurrentHp
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(xs =>
            {
                if (xs == 0)
                {
                    gameController.PlayerDeath(gameObject);
                }
            }).AddTo(disposables);
    }

    
    public void OnTriggerEnter(Collider other)
    {
        gameController.TriggerForPlayer(other, gameObject);
    }

    public void OnDestroy()
    {
        Debug.Log("player destroy");
        disposables.Dispose();

    }

}
