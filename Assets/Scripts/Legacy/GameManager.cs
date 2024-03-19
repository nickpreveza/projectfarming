using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using Unity.VisualScripting;

public class GameManager : SingletonPersistent<GameManager>
{
    public enum HempScene
    {
        TreeHouse,
        Harbor,
        GeorgiosTestScene,
        TransitionTest,
        None
    };

    public PlayerManager playerPrefab;
    public Button Inventory;
    private InputManager inputManager;
    private static bool isPaused = false;
    private static bool inventoryIsOpen = false;
    public GameObject pauseScreen;
    public GameObject inventoryScreen;
    public static GameData gameData;
    private static string previousScene = "None";
    private float fogUpdateTimer;
    private float fogUpdateInterval = 0.08f;
    [HideInInspector]
    //public static HempScene PreviousScene { set { previousScene = value; } get { return previousScene; } }

    protected override void Awake()
    {
        base.Awake();
        LoadGame();
        SpawnPlayer();
    }
    private void Start()
    {
        
        Inventory.onClick.AddListener(onClickInventory);

        inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();
        if (pauseScreen == null)
            Debug.Log("no Pause screen added to GameManager");
    }
    private void Update()
    {

        if (inputManager.GetPasuseScreenInput())
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
                ToggleInventory(false);
                //ToggleBackpack(false);
            }
        }

        if (isPaused == false)
        {
            if (inputManager.GetInventoryScreenToggle())
            {
                ToggleInventory(!inventoryIsOpen);
            }

            if (fogUpdateTimer <= 0)
            {
                //FoW.FogOfWarTeam.GetTeam(0).ManualUpdate(fogUpdateInterval);
                fogUpdateTimer = fogUpdateInterval;
            }
            else
            {
                fogUpdateTimer -= Time.deltaTime;
            }
        }


    }

    public void LoadNextScene(HempScene nextScene)
    {
        string scene = SceneManager.GetActiveScene().name;
        SaveGame(scene); //save current state before loading new scene
        previousScene = scene;
        SceneManager.LoadScene(nextScene.ToString(), LoadSceneMode.Single);
    }

    private void SpawnPlayer()
    {
        GameObject[] allSpawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPoint");
        SceneTransition transition = null;
        foreach (GameObject spawnPoint in allSpawnPoints)
        {
            transition = spawnPoint.GetComponentInParent<SceneTransition>();
            if (transition.nextScene.ToString() == previousScene)
            {
                PlayerManager player = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
                CinemachineVirtualCamera camera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
                camera.Follow = player.transform;

            }
        }
    }


    private void LoadGame()
    {
        gameData = SaveLoad.LoadGameData();
        if (gameData == null)
            gameData = new GameData();
        /*
        if (SceneManager.GetActiveScene().name == HempScene.GeorgiosTestScene.ToString())
        {
            if (gameData.fogDataThreeHouse != null && gameData.fogDataThreeHouse.Length > 0)
                FoW.FogOfWarTeam.GetTeam(0).SetTotalFogValues(gameData.fogDataThreeHouse);
        }
        else if (SceneManager.GetActiveScene().name == HempScene.TransitionTest.ToString())
        {
            if (gameData.fogDataHarbor != null && gameData.fogDataHarbor.Length > 0)
                FoW.FogOfWarTeam.GetTeam(0).SetTotalFogValues(gameData.fogDataHarbor);
        }*/

    }

    public void SaveGame(string scene)
    {
        //gameData.SetFogData(scene);
        SaveLoad.SaveGameData(gameData);
    }

    public void DeleteSaveGame()
    {
        SaveLoad.DeleteFile();
    }

    public void ToggleInventory(bool active)
    {
        inventoryIsOpen = active;
        inventoryScreen.SetActive(active);
    }

    public void ToggleBackpack(bool active)
    {
        inventoryIsOpen = active;
        inventoryScreen.SetActive(active);
    }
    void onClickInventory()
    {
        ToggleBackpack(!inventoryIsOpen);
    }
    //void onClickInventory()
    //{
    //    Backpack.SetActive(true);
    //    if ( backPackOn == false)
    //    {
    //        Backpack.SetActive(true);
    //        backPackOn = true;
    //    }
    //    else
    //    {
    //        Backpack.SetActive(false);
    //        backPackOn = false;
    //    }
    //}

    public void PauseGame()
    {
        isPaused = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        isPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void Settings()
    {
        RectTransform[] transforms = pauseScreen.GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform rect in transforms)
        {
            if (rect.gameObject.name.CompareTo("ButtonPanel") == 0)
            {
                rect.gameObject.SetActive(false);
            }
            else if (rect.gameObject.name.CompareTo("SoundPanel") == 0)
            {
                rect.gameObject.SetActive(true);
            }
        }
    }
    public void Back()
    {
        RectTransform[] transforms = pauseScreen.GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform rect in transforms)
        {
            if (rect.gameObject.name.CompareTo("ButtonPanel") == 0)
            {
                rect.gameObject.SetActive(true);
            }
            else if (rect.gameObject.name.CompareTo("SoundPanel") == 0)
            {
                rect.gameObject.SetActive(false);
            }
        }
    }

    public static void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}

