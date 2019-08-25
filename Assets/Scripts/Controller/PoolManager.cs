using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager 
{
    private static Dictionary<string, Queue<GameObject>> poolsDictionary;    
    private static Transform activatedObjectsParent;


    public static void Init(Transform dispooledObjectsContainer) //передать ГО, который не будет уничтожаться
    {
        
        activatedObjectsParent = dispooledObjectsContainer;
        poolsDictionary = new Dictionary<string, Queue< GameObject >> ();
    }



    public static GameObject Get(GameObject prefab)  // получить объект из пула
    {
        if (!poolsDictionary.ContainsKey(prefab.name))
        {
            poolsDictionary[prefab.name] = new Queue<GameObject>();
        }

        GameObject result;

        if (poolsDictionary[prefab.name].Count > 0)
        {
            
            result = poolsDictionary[prefab.name].Dequeue();
            result.SetActive(true);
           
           
            return result;
        }

        result = GameObject.Instantiate(prefab);
        result.name = prefab.name;
        result.transform.parent = activatedObjectsParent;

        return result;
    }

    public static void Put(GameObject target) //положить объект в пул
    {
        poolsDictionary[target.name].Enqueue(target);
        target.SetActive(false);
        
        
    }



}
