using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    normalGun,
    waterGun,

};
public enum BulletType
{
    normal,
    water,

};
/// <summary>
/// later add an item level enum
/// </summary>
public class GunParameters : MonoBehaviour
{
    public int bulletCapacity;
    public int bulletAmmount;
    bool refillable;
    public GunType gunType;
    public BulletType bulletType;
    public GameObject normalBullet;
    public GameObject waterBullet;
    public GameObject Player;
    public bool canRefillWater = false;
    public bool emptyWater = false;
    public GameObject bulletSpawnPoint;

    public Vector3 mousePos;
    private InputManager inputManager;
    
    private void Start()
    {
        inputManager = Player.GetComponent<InputManager>();
    }
    private void Update()
    {
        if (gunType == GunType.waterGun)
        {
            RefillWaterGun();
        }
        GunPositionChange();
    }
    void RefillWaterGun()
    {
        if(canRefillWater==true&& inputManager.GetRefillInput())
        {
            bulletAmmount++;
            if(bulletAmmount==bulletCapacity)
            {
                return;
            }
        }
    }
    void GunPositionChange()
    {
        if (Player.GetComponent<PlayerAnimator>().direction == new Vector2(0, 0))
        {
            return;
        }
        else
        {
            this.gameObject.transform.position = Player.transform.position ;
        }
    }
    
}
