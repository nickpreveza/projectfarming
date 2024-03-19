using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    private bool shooterIsPlayer;
    public Pool.PoolType poolType;
    public float bulletDamage;
    
    private bool IsShooterPlayer()
    {
        if (GetComponent<PlayerManager>())
            return true;
        return false;
    }
    public void SetShooter(GameObject shooter)
    {
        if (shooter.GetComponent<PlayerManager>())
        {
            shooterIsPlayer = true;
        }
        else
        {
            shooterIsPlayer = false;
        }
    }

    public void SetDamage(GameObject shooter)
    {
        if (shooter.GetComponent<Enemy>() is OilBoss oilBoss)
            bulletDamage = oilBoss.enemyData.attackDamage;
        else
            bulletDamage = 1f;
    }
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        { 
            Debug.LogError("No rigidbody on bullet");
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (shooterIsPlayer)
        {
            if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "HempPlant" && collision.gameObject.tag != "Item")
            {
                Kill();
            }
            else
            {
                
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
            }
        }
        else
        {
            if(collision.gameObject.tag == "Player")
            {
                PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
                player.Health -= bulletDamage;
                Kill();
            }
            else if (collision.gameObject.tag != "HempPlant" && collision.gameObject.tag != "Item")
            {
                Kill();
            }
        }
    }

    private void OnBecameInvisible()
    {
        Kill();
    }

    public void Kill()
    {
        if (gameObject.activeSelf)
        {
            PoolManager.Instance.GetPool(poolType).KillObject(this.gameObject);
        }
    }
}
