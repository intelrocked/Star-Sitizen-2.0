using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class GameView : GameElement
{
    public GameModel gameModel;
    public GameController gameController;
  

    public Button[] LvlOnMapButton;
    public Text[] lvlStatusText;
    

    public Button playButton;
    public Button playAgainButton;

    public GameObject mapPanel;
    public GameObject menuPanel;
    public GameObject deathPanel;

    public GameObject[] hpImage;


    private CompositeDisposable disposables = new CompositeDisposable();


    void Start()
    {       

        gameController = app.controller;

       
        app.model.playerSettings.CurrentHp
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(xs =>
            {
                
                for (int i = app.model.playerSettings.playerHP; i > xs; i--)
                {
                    
                    Debug.Log(xs);
                    hpImage[i-1].SetActive(false);
                    
                }
                for (int i = 0; i < xs; i++)
                {

                    Debug.Log(xs);
                    hpImage[i].SetActive(true);

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



    


