using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "ObjectPool", menuName = "ScriptableObjects/ObjectPools")]
public class Pool : ScriptableObject
{
    [HideInInspector]
    public enum PoolType
    {
        Crickets,
        PlayerBullets,
        EnemyBullets,
        OilMonsters,
        OilBullets
    };
    [SerializeField] private GameObject prefab;
    public PoolType type;
    [Tooltip("If set too low. allocation needs to be increased in runtime. /n" +
        "If set too high it will allocate too much memory. Find the sweet spot")]
    public int startCapacity = 100;
    public int maxCapacity = 1000;
    private ObjectPool<GameObject> objectPool;

    public bool usingPool { get; private set; }

    public void CreatePool(Transform parent, Pool pool)
    {
        if (prefab == null)
        {
            usingPool = false;
            Debug.LogError("Prefab not added to Pool");
        }
        else
        {
            usingPool = true;
            objectPool = new ObjectPool<GameObject>(() =>
            {
                GameObject newObject = Instantiate(prefab);
                newObject.transform.parent = parent;
                return newObject;
            }, thisObject =>
            {
                thisObject.gameObject.SetActive(true);
            }, thisObject =>
            {
                thisObject.gameObject.SetActive(false);
            }, thisObject =>
            {
                Destroy(thisObject.gameObject);
            }, true, startCapacity, maxCapacity);
        }
    }

    public GameObject Get()
    {
        return objectPool.Get();
    }

    public void KillObject(GameObject objectToKill)
    {
        if (usingPool)
            objectPool.Release(objectToKill);
        else
            Destroy(objectToKill.gameObject);
    }
}
