using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManagerOLD : MonoBehaviour
{
    [System.Serializable]
    public class WaveEnemy
    {
        public Enemy enemy;
        public int spawnCost = 1;
    }

    public TMP_Text nextWaveClock;
    public TMP_Text currentWaveClock;
    public TMP_Text currentWaveText;
    public TMP_Text enemiesDisplay;
    [Header("Time between waves in seconds ")]
    public int waveInterval = 60;
    [Header("Duration of a wave in seconds")]
    public int waveDuration = 10;
    public List<WaveEnemy> enemies = new List<WaveEnemy>();
    public List<WaveEnemy> bosses = new List<WaveEnemy>();
    public int currentWave { get; private set; }
    public EnemySpawner[] spawnLocations;

    private int waveMoney;              //Purchasing power / spawning power
    private int moneyMultiplier = 10;   //this is multiplied with currentwave.
    private List<Enemy> enemiesToSpawn = new List<Enemy>();

    private float nextWaveTimer;
    private bool isWaveActive = false;
    private float currentWaveTimer;
    private float spawnInterval = 3;
    private float spawnTimer;
    private int bossGeneration = 2; //every 10th wave

    private void Awake()
    {
        if (spawnLocations.Length <= 0)
        {
            Debug.LogError("No spawnlocations added to WaveManager");
        }
        if (enemies.Count <= 0)
        {
            Debug.LogError("No enemy Prefabs added to WaveManager");
        }
        if (nextWaveClock == null || currentWaveClock == null || currentWaveText == null || enemiesDisplay == null)
        {
            Debug.LogError("TextMeshPro elements needs to be added to Wavemanager to display text correctly");
        }
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            spawnLocations[i].SetInWaveManager(true);
        }
    }
    private void Start()
    {
        currentWave = GameManager.gameData.wavesCompleted;
        currentWaveText.text = "" + currentWave;
        enemiesDisplay.text = "0";
        nextWaveTimer = GameManager.gameData.nextWaveTime;
        if (nextWaveTimer == 0)
            nextWaveTimer = waveInterval;
        isWaveActive = false;
    }

    private void Update()
    {
        UpdateClocks();

        if (nextWaveTimer <= 0 && isWaveActive == false)
        {
            GenerateWave();
            currentWaveTimer = waveDuration;
            isWaveActive = true;
            nextWaveTimer = waveInterval;
            GameManager.gameData.nextWaveTime = nextWaveTimer;
        }
        else if(nextWaveTimer <= 0 && isWaveActive)
        {
            nextWaveTimer = 0f;
            GameManager.gameData.nextWaveTime = nextWaveTimer;
        }
        else
        {
            nextWaveTimer -= Time.deltaTime;
            GameManager.gameData.nextWaveTime = nextWaveTimer;
        }
        if (isWaveActive)
        {
            UpdateWave();
        }
        
    }

    private void UpdateClocks()
    {
        int minutes = (int)nextWaveTimer / 60;
        int seconds = (int)nextWaveTimer % 60;
        nextWaveClock.text = GetFormatedTime(minutes, seconds);

        minutes = (int)currentWaveTimer / 60;
        seconds = (int)currentWaveTimer % 60;
        currentWaveClock.text = GetFormatedTime(minutes, seconds);
    }

    private string GetFormatedTime(int minutes, int seconds)
    {
        string minutesText = minutes.ToString();
        string secondText = seconds.ToString();
        if (minutes < 10)
        {
            minutesText = "0" + minutes;
        }
        if (seconds < 10)
        {
            secondText = "0" + seconds;
        }
        return minutesText + ":" + secondText;
    }


    private void UpdateWave()
    {
        if (enemiesToSpawn.Count <= 0)
        {
            currentWaveTimer = 0; //set wavetimmer to 0 if there are no more enemies to spawn
            isWaveActive = false;
            GameManager.gameData.wavesCompleted = currentWave;
            return;
        }
        else
        {
            if (spawnTimer <= 0)
            {
                TrySpawn();
                spawnTimer = spawnInterval; //reset spawnTimer
            }
            else
            {
                spawnTimer -= Time.deltaTime;
            }
            enemiesDisplay.text = enemiesToSpawn.Count.ToString();
            currentWaveTimer -= Time.deltaTime;
        }

    }
    public void GenerateWave()
    {
        currentWave++;
        currentWaveText.text = currentWave.ToString();
        waveMoney = currentWave * moneyMultiplier;
        GenerateEnemies();
        int bossIndex = GetBossIndex();
        GenerateBoss(bossIndex);
        spawnInterval = (float)waveDuration / enemiesToSpawn.Count;
    }

    private void TrySpawn()
    {
        EnemySpawner spawner = null;
        if (enemiesToSpawn.Count > 0)
        {
            spawner = GetRandomSpawnlocation(enemiesToSpawn[0]);
            SpawnEnemy(spawner);
        }
        
    }
    private void SpawnEnemy(EnemySpawner spawner)
    {
        if (spawner)
        {
            spawner.SpawnEnemy(enemiesToSpawn[0]);
            enemiesToSpawn.RemoveAt(0);
        }
    }
    //private void SpawnEnemy(EnemySpawner spawner)
    //{

    //if (enemiesToSpawn.Count > 0)
    //{
    //    EnemySpawner spawner = GetRandomSpawnlocation(enemiesToSpawn[0]);
    //    if (spawner != null)
    //    {
    //        spawner.SpawnEnemy(enemiesToSpawn[0]);
    //        enemiesToSpawn.RemoveAt(0);
    //        spawnTimer = spawnInterval; //reset spawnTimer
    //    }
    //    else
    //    {
    //        Debug.LogError("No spawner for " + enemiesToSpawn[0] +" found!!");
    //    }
    //}
    //else
    //{
    //    currentWaveTimer = 0; //set wavetimmer to 0 if there are no more enemies to spawn
    //    nextWaveTimer = waveInterval;
    //    isWaveActive = false;
    //}
    //}

    private EnemySpawner GetRandomSpawnlocation(Enemy enemy)
    {
        int randomNum;
        for (int i = 0; i < 10; i++) //run for max 10 times too find a random spawner that has a spawnspot left. so its not searched for infinity
        {
            randomNum = Random.Range(0, spawnLocations.Length);
            if (spawnLocations[randomNum].HasSpawnSpotsLeft() && spawnLocations[randomNum].CanBeSpawnedHere(enemy.enemyData.enemyType))
            {
                return spawnLocations[randomNum];
            }
        }
        return null;
    }

    private void GenerateEnemies()
    {
        //List<Enemy> GeneratedEnemies = new List<Enemy>();
        int minimumCost = GetMinimumCost();
        while (waveMoney > 0)
        {
            int randomEnemyID = Random.Range(0, enemies.Count);
            int randomEnemyCost = enemies[randomEnemyID].spawnCost;

            if (waveMoney - randomEnemyCost >= 0 && waveMoney >= minimumCost)
            {
                enemiesToSpawn.Add(enemies[randomEnemyID].enemy);
                waveMoney -= randomEnemyCost;
            }
            else if (waveMoney <= 0 || waveMoney < minimumCost)
            {
                break;
            }
        }

        //enemiesToSpawn = GeneratedEnemies;

    }
    private int GetMinimumCost()
    {
        int min = 99999;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].spawnCost < min)
                min = enemies[i].spawnCost;
        }
        return min;
    }
    private int GetBossIndex()
    {
        if (currentWave % bossGeneration == 0)
            return (currentWave / bossGeneration) - 1;
        return -1;
    }
    private void GenerateBoss(int bossIndex)
    {
        if (bossIndex >= 0 && bossIndex < bosses.Count)
        {
            enemiesToSpawn.Add(bosses[bossIndex].enemy);
        }

    }

    //private Enemy GetEnemyToSpawn()
    //{
    //    Enemy enemy = null;
    //    if (enemiesToSpawn[0] is Cricket)
    //        enemy = GetEnemyFromPool(Pool.PoolType.Crickets);
    //    if (enemiesToSpawn[0] is CricketQueen)
    //        enemy = Instantiate(enemiesToSpawn[0]);

    //    if (enemy == null) //if enemy not found or not using any pool
    //        enemy = Instantiate(enemiesToSpawn[0]);
    //    return enemy;
    //}

    //private Enemy GetEnemyFromPool(Pool.PoolType type)
    //{
    //    //Get Enemy from pool
    //    if (PoolManager.Instance.GetPool(type).usingPool)
    //    {
    //        return PoolManager.Instance.GetPool(type).Get().GetComponent<Enemy>();
    //    }
    //    return null;
    //}
}
