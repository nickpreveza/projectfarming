using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public bool waterGunActive, normalGunActive;
    public GameObject waterGun;
    public GameObject normalGun;
    public GameObject hempPlant;
    int maxPlanted=0;

    private InputManager inputManager;
    public HealthBar healthBar;
    private float health = 100f;
    private void Awake()
    {

        inputManager = this.GetComponent<InputManager>();
        healthBar = FindObjectOfType<HealthBar>();
        if (healthBar == null)
        {
            Debug.LogWarning("Add a healthbar to UI and link that to the " + this);
        }
        else
        {
            //Health = health; //Note(Guillem): This is completely unnecessary
            //healthBar.SetMaxHealth(health);
        }


    }
    private void Start()
    {
        //healthBar.UpdateHealthbar(health);
    }
    void Update()
    {
        ActivateWaterGun();
        ActivateNormalGun();
        if (CheckPlantingInput())
        {
            PlantSeeds();
        }
    }

    public void UpdateHealthBar()
    {
        //if (healthBar != null)
        //healthBar.UpdateHealthbar(health);
    }
    // health and kill finction
    public float Health
    {
        set
        {
            health = value;
            UpdateHealthBar();
            if (health <= 0f)
            {
                Killed();
            }
        }
        get
        {
            return health;
        }
    }

    private void Killed()
    {
        Destroy(gameObject);
    }
    //
    // check for active guns
    int n = 0;
    void ActivateWaterGun()
    {

        if (inputManager.GetWaterGunActiveInput() && n > 0)
        {
            waterGun.SetActive(false);
            waterGunActive = false;
            n = 0;
        }
        else if (inputManager.GetWaterGunActiveInput())
        {
            waterGunActive = true;
            normalGunActive = false;
            waterGun.SetActive(true);
            normalGun.SetActive(false);
            n++;
        }
    }
    int i = 0;
    void ActivateNormalGun()
    {
        if (inputManager.GetNormalGunActiveInput() && i > 0)
        {
            normalGun.SetActive(false);
            normalGunActive = false;
            i = 0;
        }
        else if (inputManager.GetNormalGunActiveInput())
        {
            waterGunActive = false;
            normalGunActive = true;
            waterGun.SetActive(false);
            normalGun.SetActive(true);
            i++;
        }
    }
    //

    //private void OnTriggerEnter2D(Collider2D collision) // add player trigger collider   this will be in water function
    //{
    //    if (this.GetComponentInChildren<GunParameters>().gunType == GunType.waterGun && this.GetComponentInChildren<GunParameters>().emptyWater==true)
    //    {
    //        if (collision.tag == "Water" )
    //        {
    //            this.GetComponentInChildren<GunParameters>().canRefillWater = true;
    //        }
    //    }

    //}

    // farming functions
    bool CheckPlantingInput()
    {
        if (inputManager.GetGlobalPlantInput())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void PlantSeeds()
    {
        GameObject[] plantingSpots = GameObject.FindGameObjectsWithTag("PlantingSpot");
        for (int i = 0; i < plantingSpots.Length; i++)
        {

            if (maxPlanted == plantingSpots.Length)
            {
                return;
            }
            else
            {
                Instantiate(hempPlant, plantingSpots[i].GetComponent<Transform>().position, plantingSpots[i].GetComponent<Transform>().rotation);
                maxPlanted++;
            }
        }
    }
    //
}
