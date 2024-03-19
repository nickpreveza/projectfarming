using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private bool isSpawned;
    private float spawnTimer = 0f;
    [Header("Spawn rate if it's not part oof the waveManager")]
    public float spawnRate = 1f; //rate of spawns in seconds
    public GameObject EnemyPrefab;
    public Enemy enemy { get; private set; }
    public EnemyData.EnemyType spawnType { get; private set; }
    public int maxEnemiesToSpawn = 4;
    public bool canBeDestroyed = false;
    public float health = 0;
    private bool isInWaveManager = false;
    private List<Enemy> spawnedEnemies;
    private OilBoss owner;

    private void Awake()
    {
        enemy = EnemyPrefab.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("No Enemy prefabs added to EnemySpawner");
        }
        else
        {
            isSpawned = true;
            spawnType = enemy.enemyData.enemyType;
            spawnTimer = spawnRate;
        }
        
        spawnedEnemies = new List<Enemy>();
    }
    public void Create(OilBoss oilBoss, EnemyData.EnemyType enemyType)
    {
        owner = oilBoss;
        if (spawnType == enemyType)
        {
            canBeDestroyed = true;
            health = 10;
        }
    }

    public float Health
    {
        set
        {
            health = value;
            if (health <= 0f)
            {
                if (canBeDestroyed)
                {
                    Killed();
                }
            }
        }
        get
        {
            return health;
        }
    }

    public void SetInWaveManager(bool isInWave)
    {
        isInWaveManager = isInWave;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInWaveManager == false)
        {
            if (isSpawned)
            {
                if (spawnTimer <= 0f)
                {
                    spawnTimer = spawnRate;
                    isSpawned = false;
                }
                else
                {
                    spawnTimer -= Time.deltaTime;
                }
            }
            else
            {
                isSpawned = SpawnEnemy(enemy);
            }
        }
    }

    public bool SpawnEnemy(Enemy enemyToSpawn)
    {
        if (spawnedEnemies.Count < maxEnemiesToSpawn)
        {
            Enemy newEnemy = GetEnemyToSpawn(enemyToSpawn);
            newEnemy.SetSpawnLocation(this);
            spawnedEnemies.Add(newEnemy);
            return true;
        }
        return false;
    }

    public void Kill(Enemy enemy)
    {
        spawnedEnemies.Remove(enemy);
        enemy.SetSpawnLocation(null);
        if (enemy is Cricket)
            PoolManager.Instance.GetPool(Pool.PoolType.Crickets).KillObject(enemy.gameObject);
        else if (enemy is OilMonster)
            PoolManager.Instance.GetPool(Pool.PoolType.OilMonsters).KillObject(enemy.gameObject);
    }

    public bool HasSpawnSpotsLeft()
    {
        if (spawnedEnemies.Count < maxEnemiesToSpawn)
            return true;
        return false;
    }

    public bool CanBeSpawnedHere(EnemyData.EnemyType enemyToSpawn)
    {
        if (spawnType == enemyToSpawn)
            return true;
        else if (spawnType == EnemyData.EnemyType.Cricket && enemyToSpawn == EnemyData.EnemyType.CricketQueen)
        {
            return true;
        }
        return false;
    }

    private Enemy GetEnemyToSpawn(Enemy enemyToSpawn)
    {
        Enemy enemy = null;

        if (enemyToSpawn is Cricket)
            enemy = GetEnemyFromPool(Pool.PoolType.Crickets);
        else if (enemyToSpawn is CricketQueen)
            enemy = Instantiate(enemyToSpawn);
        else if(enemyToSpawn is OilMonster)
            enemy = GetEnemyFromPool(Pool.PoolType.OilMonsters);

        if (enemy == null) //if enemy not found or not using any pool
            enemy = Instantiate(enemyToSpawn);
        return enemy;
    }

    private Enemy GetEnemyFromPool(Pool.PoolType type)
    {
        //Get Enemy from pool
        if (PoolManager.Instance.GetPool(type).usingPool)
        {
            return PoolManager.Instance.GetPool(type).Get().GetComponent<Enemy>();
        }
        return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet" && gameObject.activeSelf)
        {
            Health--;
            foreach(Enemy enemy in spawnedEnemies)
            {
                enemy.Aggressiveness += 50; //if  spawner is attacked all enemies from here go to defend it
            }
        }
    }

    protected void Killed()
    {
        if(owner is OilBoss oilBoss)
        {
            oilBoss.DestroyOilWell(this);
        }
        else
            Destroy(this.gameObject);
    }
}
