using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
public class ShootingManager : MonoBehaviour
{
    private GameObject playerBullet;
    public GameObject bulletPrefab;
    public float shootingInterval;
    float bulletSpeed;
    public Transform playerPos;
    GameObject Player;
    public bool waterGunOn, normalGunOn;
    private GameObject normalGun;

   public bool isShooting = false;
    //bool gunInHand = false; later when you can select items
    public List<BulletParameters> currentBullets = new List<BulletParameters>();


    public Animator playerAnimator;
    private InputManager inputManager;
    private void Start()
    {
       
        Player = GameObject.FindGameObjectWithTag("Player");
        inputManager = Player.GetComponent<InputManager>();
        normalGun = GetComponentInParent<PlayerManager>().normalGun;

    }
    private void Update()
    {
       
        isShooting = CheckInput();
        if (isShooting == true)
        {
            BulletCreation();
        }
    }
    bool CheckInput()
    {
        waterGunOn = Player.GetComponent<PlayerManager>().waterGunActive;
        normalGunOn = Player.GetComponent<PlayerManager>().normalGunActive;
        if ( waterGunOn)
        {
            if (inputManager.GetShootingWaterGunInput())
            {
               
            }
            else 
            {
                return false;
            }
        }
        if (normalGunOn)
        {
            if (inputManager.GetShootingNormalGunInput())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }
    void BulletCreation()
    {
        BulletParameters bulletInstant;
        if (normalGunOn)
        {
            if (PoolManager.Instance.GetPool(Pool.PoolType.PlayerBullets).usingPool)
            {
                bulletInstant = PoolManager.Instance.GetPool(Pool.PoolType.PlayerBullets).Get().GetComponent<BulletParameters>();
                bulletInstant.transform.position = normalGun.transform.position;
                //bulletInstant.transform.rotation = Player.GetComponentInChildren<Transform>().rotation;
                bulletInstant.tag = "PlayerBullet";
                currentBullets.Add(bulletInstant);
            }
        }
    }
   

}
