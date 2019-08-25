using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager 
{
    private static Dictionary<string, Queue<GameObject>> poolsDictionary;
    private static Transform deactivatedObjectsParent;
    private static Transform activatedObjectsParent;


    public static void init(Transform pooledObjectsContainer, Transform dispooledObjectsContainer) //передать ГО, который не будет уничтожаться
    {
        deactivatedObjectsParent = pooledObjectsContainer;
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
            Debug.Log("jovanii rot etogo kazino");
            result = poolsDictionary[prefab.name].Dequeue();
            result.SetActive(true);
            //result.transform.parent = activatedObjectsParent;
           
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
        //target.transform.parent = deactivatedObjectsParent;
        
    }



}
