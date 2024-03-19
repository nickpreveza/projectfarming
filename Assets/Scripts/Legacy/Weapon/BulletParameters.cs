using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParameters : MonoBehaviour
{
    public Rigidbody2D bulletRb;
    public float speed;
    public int damage;
    GameObject Player;
    ShootingManager shootingManager;
    public Vector3 direction;
    public Vector3 rotation;
    public float movementAngle;
    bool hasMoved = false;
     void OnEnable()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        shootingManager = Player.GetComponent<ShootingManager>();
        direction = Player.GetComponent<PlayerAnimator>().mouseInput;
        if (direction == new Vector3(0,0))
        {
            direction = Player.GetComponent<PlayerAnimator>().mouseInput;
        }
        
        bulletRb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        hasMoved = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

    private void OnBecameInvisible()
    {
        Kill();
    }

    public void Kill()
    {
        if (gameObject.activeSelf && hasMoved)
        {
            shootingManager.currentBullets.Remove(this);
            PoolManager.Instance.GetPool(Pool.PoolType.PlayerBullets).KillObject(this.gameObject);
        }
    }
}
