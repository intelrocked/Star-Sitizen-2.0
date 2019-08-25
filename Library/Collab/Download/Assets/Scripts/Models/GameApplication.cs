using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameApplication : MonoBehaviour
{
 
    public GameModel model = new GameModel();
    public GameView view;
    public GameController controller;
    
}

public class GameElement : MonoBehaviour
{
    private GameApplication application;
    public GameApplication app { get { return application; } }

    void Awake()
    {
        application = GameObject.FindObjectOfType<GameApplication>();
    }
}
