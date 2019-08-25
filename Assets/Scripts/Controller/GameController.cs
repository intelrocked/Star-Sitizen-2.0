using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;
using System.Threading.Tasks;

public class GameController : GameElement
{
    public GameModel gameModel;
    public GameView gameView;
    public BoundaryModel boundaryModel;
    public PlayerSettingsModel playerModel;
    public LvlDataModel lvlDataModel;
   

    public FileSave fileSave = new FileSave();
    public PoolManager poolManager = new PoolManager();

    private LvlStruct lvlStructSource;

    public GameObject player;
    public GameObject asteroidsPref;
    public GameObject bulletPref;

    public Transform spawnAsteroidObj;
    
    public Transform bulletSpawn; 

    private GameObject tmpAster;
    private GameObject tmpBullet;

    private bool IsDead;
    
   
    private int countAsterDeath = 0;

    
   

    void Start()
    {
        gameView = app.view;
        boundaryModel = new BoundaryModel();
        playerModel = app.model.playerSettings;
        
        //write and read to XML
        gameModel = fileSave.ReadXml<GameModel>();
        
        app.model = gameModel;
        gameView.showTextAster();
        lvlDataModel = app.model.lvldata;
        //fileSave.WriteXml(gameModel);
        lvlStructSource = app.model.GetLvlStruct();

        if (lvlDataModel.lvlStruct[0].status != LvlStatus.Finished)
        {
            lvlDataModel.lvlStruct[0].status = LvlStatus.Open;
        }
        

          PoolManager.Init(spawnAsteroidObj);


        //определяем границы камеры
        boundaryModel.SetBoundary(Camera.main.ViewportToWorldPoint(new Vector2(0, 0)), Camera.main.ViewportToWorldPoint(new Vector2(1, 1)));


        //наблюдаем за добавлением объекта в коллекцию астероидов
        app.model.getAsterCollection().ObserveAdd().Subscribe(x =>
        {
            GameObject obj = PoolManager.Get(asteroidsPref);            
            obj.GetComponent<AsteroidView>().setModel(x.Value);
            x.Value.moveObject();        
            
        });

        //наблюдаем за удалением объекта в коллекции астероидов
        app.model.getAsterCollection().ObserveRemove().Subscribe(x =>
        {
           
            PoolManager.Put(tmpAster);
        });

        //наблюдаем за добавлением объекта в коллекцию пуль
        app.model.getBulletCollection().ObserveAdd().Subscribe(x =>
        {
            GameObject obj = PoolManager.Get(bulletPref);
            obj.GetComponent<BulletView>().setModel(x.Value);
            x.Value.moveObject();

        });

        //наблюдаем за удалением объекта в коллекции пуль
        app.model.getBulletCollection().ObserveRemove().Subscribe(x =>
        {
            
            PoolManager.Put(tmpBullet);
        });

        

        Observable.EveryUpdate() 
            .Where(_ => Input.anyKeyDown) 
            .Select(_ => Input.inputString) 
            .Subscribe(x => { 
                OnKeyDown(x); 
            }).AddTo(this);
        
    }

   

    private void OnKeyDown(string keyCode)
    {
        switch (keyCode)
        {
            case "e":
                BulletModel bullet = new BulletModel(bulletSpawn.transform.position, playerModel.bulletSpeed);
                gameModel.setBulletCollection(bullet);
                break;
            default:
                
                break;
        }
    }

    public Vector3 RandomPositionSpawnAseter()
    {
        Vector3 randomPos = new Vector3((Random.Range(boundaryModel.min.x, boundaryModel.max.x)), boundaryModel.max.y + 2, 0.0f);
        return randomPos;
    }
    
    

    private IEnumerator AsteroidsSpawn()
    {
        LvlStruct lvlStructSource = app.model.GetLvlStruct();
            for (int i = 0; i < lvlStructSource.asteroidsCount; i++)
            {
                AsteroidModel asteroid = new AsteroidModel(lvlStructSource.asteroidsHP, RandomPositionSpawnAseter(), lvlStructSource.asteroidsSpeed);
                gameModel.setAsterCollection(asteroid);


                yield return new WaitForSeconds(lvlStructSource.CoroutineRate);

            }

    }

    //тригеры для астероида
    public void TriggerDeathZ(Collider other, AsteroidModel model, GameObject obj)
    {
        if (other.CompareTag("DeathZone"))
        {

            DeathTrigger(model, obj);
        }
        if (other.CompareTag("Bullet"))
        {
            
            model.CurrentHp.Value--;
            if (model.CurrentHp.Value == 0)
            {
                DeathTrigger(model, obj);
            }
        }


    }

    public void DeathTrigger(AsteroidModel model, GameObject obj)
    {
        countAsterDeath++;
        lvlDataModel.currentAsteroid.Value = countAsterDeath;
        tmpAster = obj;
        gameModel.delAsterCollection(model);
        LvlLog();
    }

    //тригеры для пули
    public void TriggerBulletDeath(Collider other, BulletModel model, GameObject obj)
    {
        if (other.CompareTag("DeathZone" ) || other.CompareTag("Asteroid") ) 
        {
           
            tmpBullet = obj;
            app.model.delBulletCollection(model);

        }        
    }

    //тригеры для игрока
    public void TriggerForPlayer(Collider other,  GameObject obj)
    {
        if (other.CompareTag("Asteroid"))
        {
            playerModel.ReduceHp();
        }
    }

    

    //смерть игрока
    public void PlayerDeath(GameObject gameObject)
    {
        Debug.Log("You Died");
        IsDead = true;
        Destroy(gameObject);
    }

 

    //выбор уровня
    public void ChooseLvl(int Number)
    {
        IsDead = false;        
        app.model.SetCurrentLvl(Number+1);
        player.SetActive(true);
        countAsterDeath = 0;        
        GenerateLvl();
        fileSave.WriteXml(app.model);
        StartCoroutine(AsteroidsSpawn());
      
    }

    //генерация параметров уровня
    public void GenerateLvl()
    {
        if (IsDead != true)
        {
            ref LvlStruct lvlStructSource = ref app.model.GetLvlStruct();
            if (lvlStructSource.status == LvlStatus.Close)
            {
                lvlStructSource.status = LvlStatus.Open;
            }
            lvlDataModel.currentAsteroid.Value = 0;
            lvlStructSource.asteroidsCount = Random.Range(10, 30);
            lvlDataModel.maxCurAster.Value = lvlStructSource.asteroidsCount;
            lvlStructSource.asteroidsHP = Random.Range(1, 5);
            lvlStructSource.CoroutineRate = Random.Range(0.3f, 1f);
            lvlStructSource.asteroidsSpeed = Random.Range(1, 4);
        }
        
    }

    public void LvlLog()
    {
        ref LvlStruct lvlStructSource = ref app.model.GetLvlStruct();
        if (countAsterDeath == lvlStructSource.asteroidsCount)
        {
            if (IsDead != true)
            {
                if (lvlDataModel.GetCurrentLvl() < lvlDataModel.allCountLvl)
                {                                        
                    playerModel.SetCurHp(playerModel.playerHP);
                    ChooseLvl(lvlDataModel.GetCurrentLvl());
                    gameView.ShowLvlStatus();
                }
                else
                {                    
                    gameView.ActivateFinalPanel();
                    Debug.Log("end game");
                }
                lvlStructSource.status = LvlStatus.Finished;
                fileSave.WriteXml(app.model);
            }
            
        }
    }


    

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (player != null )
        {
            player.GetComponent<Rigidbody>().velocity = new Vector3(moveHorizontal, moveVertical, 0f) * playerModel.playerSpeed;

            player.GetComponent<Rigidbody>().position = new Vector3
            (
              Mathf.Clamp(player.GetComponent<Rigidbody>().position.x, boundaryModel.min.x, boundaryModel.max.x),
              Mathf.Clamp(player.GetComponent<Rigidbody>().position.y, boundaryModel.min.y, boundaryModel.max.y),
              0.0f

            );
        }       



    }

    public void OnDestroy()
    {
        fileSave.WriteXml(app.model);
    }
}
