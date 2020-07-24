using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameObjectPool : MonoBehaviour
{

    private static readonly GameObjectPool instance = new GameObjectPool();

    private static readonly object padlock = new object();
    Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>() { };

    public static GameObjectPool Instance()
    {
        return instance;
    }

    void Start()
    {

    }

    public GameObject GetPool(GameObject gameObject, Vector3 position)
    {
        string key = gameObject.name;
        GameObject container; 
        if (pool.ContainsKey(key) && pool[key].Count > 0)
        {
            container = pool[key][0];
            pool[key].RemoveAt(0);
        }
        else if (pool.ContainsKey(key) && pool[key].Count <= 0)
        {
            container = Instantiate(gameObject, position, Quaternion.identity) as GameObject;
        }
        else  
        {
            container = Instantiate(gameObject, position, Quaternion.identity) as GameObject;
            pool.Add(key, new List<GameObject>() { });
        }
        container.name = key;
        container.SetActive(true);
        container.transform.position = position;
        return container;
    }

    public void IntoPool(GameObject gameObject ,string key)
    {
        if(pool.ContainsKey(key) == false)
        {
            pool.Add(key, new List<GameObject>());
            pool[key].Add(gameObject);
        }
        else
        {
            pool[key].Add(gameObject);
        }
        gameObject.SetActive(false);
    }

}
