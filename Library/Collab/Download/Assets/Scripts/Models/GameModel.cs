using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


[System.Serializable]
public class GameModel
{
    public LvlDataModel lvldata = new LvlDataModel();
    public PlayerSettingsModel playerSettings = new PlayerSettingsModel();

    private ReactiveCollection<AsteroidModel> asteroidsCollection = new ReactiveCollection<AsteroidModel>();
    private ReactiveCollection<BulletModel> bulletCollection = new ReactiveCollection<BulletModel>();


    public ReactiveCollection<AsteroidModel> getAsterCollection()
    {
        return asteroidsCollection;
    }

    public void setAsterCollection(AsteroidModel asteroidModel)
    {
        asteroidsCollection.Add(asteroidModel);
    }

    public void delAsterCollection(AsteroidModel asteroidModel)
    {
        asteroidsCollection.Remove(asteroidModel);
    }

    public ReactiveCollection<BulletModel> getBulletCollection()
    {
        return bulletCollection;
    }

    public void setBulletCollection(BulletModel bulletModel)
    {
        bulletCollection.Add(bulletModel);
    }

    public void delBulletCollection(BulletModel bulletModel)
    {
        bulletCollection.Remove(bulletModel);
    }

    public int getCountAsteroids()
    {
        return lvldata.lvlStruct[lvldata.currentLvl.Value].asteroidsCount;
    }
    
    

    

    

}




