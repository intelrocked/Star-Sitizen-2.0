using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class GameView : GameElement
{
   
    public GameController gameController;   
  

    public Button[] LvlOnMapButton;
    public Text[] lvlStatusText;
    public Text leftGoal;
    public Text rightGoal;
    public GameObject goal;

    public Button playButton;
    public Button playAgainButton;
    public Button GoToMenuButton;

    public GameObject mapPanel;
    public GameObject menuPanel;
    public GameObject deathPanel;
    public GameObject FinalPanel;

    public GameObject[] hpImage;

    private CompositeDisposable disposables = new CompositeDisposable();


    public void showTextAster()
    {
        app.model.lvldata.maxCurAster
          .ObserveEveryValueChanged(x => x.Value)
          .Subscribe(xs =>
          {
              rightGoal.text = xs.ToString();

          }).AddTo(disposables);
        app.model.lvldata.currentAsteroid
           .ObserveEveryValueChanged(x => x.Value)
           .Subscribe(xs =>
           {

               leftGoal.text = xs.ToString();

           }).AddTo(disposables);
    }

    void Start()
    {
        gameController = app.controller;        
        app.model.playerSettings.CurrentHp
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(xs =>
            {
                for (int i = 0; i < xs; i++)
                {
                    hpImage[i].SetActive(true);
                }
                for (int i =xs ; i < app.model.playerSettings.playerHP; i++)
                {
                    hpImage[i].SetActive(false);
                    
                }               
                if (xs == 0)
                {
                    deathPanel.SetActive(true);
                }

            }).AddTo(disposables);

        for (int i = 0; i < LvlOnMapButton.Length; i++)
        {
            int lvl = i;
            LvlOnMapButton[i].onClick.AddListener(() =>
            {
                if (app.model.lvldata.lvlStruct[lvl].status != LvlStatus.Close)
                {
                    gameController.ChooseLvl(lvl);
                    mapPanel.SetActive(!mapPanel.activeSelf);
                    menuPanel.SetActive(!menuPanel.activeSelf);
                    goal.SetActive(true);
                }
                else
                {
                    Debug.Log("Level closed");
                }
                
            } );
            
        }

        playButton.onClick.AsObservable().Subscribe(_ =>
        {
            mapPanel.SetActive(!mapPanel.activeSelf);
            ShowLvlStatus();
        });

        playAgainButton.onClick.AsObservable().Subscribe(_ =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        GoToMenuButton.onClick.AsObservable().Subscribe(_ =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

    }

    public void ActivateFinalPanel()
    {
        FinalPanel.SetActive(true);
    }

    public void ShowLvlStatus()
    {
        GameModel model = app.model;
        for (int i = 0; i < lvlStatusText.Length; i++)
        {
            lvlStatusText[i].text = app.model.lvldata.lvlStruct[i].status.ToString();
        }
    }

}



    


