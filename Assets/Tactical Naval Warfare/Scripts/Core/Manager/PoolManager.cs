using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [Header("Pool Configurations")]
    public List<PoolConfigData> poolConfigurations;

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var config in poolConfigurations)
        {
            if (config == null) continue;

            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < config.initialSize; i++)
            {
                GameObject obj = Instantiate(config.prefab, transform);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            poolDictionary.Add(config.poolID, objectQueue);
        }
    }

    public GameObject GetObject(string id, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(id))
        {
            Debug.LogError($"El pool '{id}' no existe.");
            return null;
        }

        GameObject obj = null;

        if (poolDictionary[id].Count > 0)
        {
            obj = poolDictionary[id].Dequeue();
        }
        else
        {
            Debug.LogWarning($"¡Te quedaste sin objetos en el pool '{id}'!.");
            return null;
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(string id, GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[id].Enqueue(obj);
    }
}
