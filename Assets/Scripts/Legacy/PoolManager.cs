using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField]
    public List<Pool> objectPools = new List<Pool>();
    public Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    protected override void Awake()
    {
        base.Awake();
        CopyListToDictionary();
    }

    private void Start()
    {
        foreach(KeyValuePair<string, Pool> pool in pools)
        {
            Transform poolContainer = CreateContainer(pool.Key.ToString());
            pool.Value.CreatePool(poolContainer.transform, pool.Value);
        }
        
    }

    public Pool GetPool(Pool.PoolType type)
    {
        Pool pool;
        
        if (pools.TryGetValue(type.ToString(), out pool))
        {
            return pool;
        }
        else
        {
            Debug.LogError("No pool with that type/name found");
            return null;
        }
    }

    private Transform CreateContainer(string name)
    {
        GameObject poolContainer = new GameObject();
        poolContainer.transform.parent = transform;
        poolContainer.name = name;
        return poolContainer.transform;
    }

    private void CopyListToDictionary()
    {
        foreach (Pool pool in objectPools)
        {
            pools.Add(pool.type.ToString(), pool);
        }
    }
}
