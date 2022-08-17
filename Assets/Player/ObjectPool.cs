using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;
    public static ObjectPool Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }

    private Dictionary<string , Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    private GameObject pool;

    public GameObject GetObject(GameObject prefab)
    {
        GameObject obj;
        if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            obj = GameObject.Instantiate(prefab);
            PushObject(obj);
            if (pool == null)
            {
                pool = new GameObject("GameobjectPool");
            }
            GameObject childPool = GameObject.Find(prefab.name + "Pool");
            if (!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }
            obj.transform.SetParent(childPool.transform);
        }
        obj = objectPool[prefab.name].Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void PushObject(GameObject obj)
    {
        string objName = obj.name.Replace("(Clone)" , string.Empty);
        if (!objectPool.ContainsKey(objName))
        {
            objectPool.Add(objName , new Queue<GameObject>());
        }
        objectPool[objName].Enqueue(obj);
        obj.SetActive(false);
    }


}
