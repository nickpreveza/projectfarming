using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 10f;
    private Pool.PoolType poolType;

    private void Awake()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Not all objects are set in the shooting script in " + transform.parent);
        }
        poolType = bulletPrefab.GetComponent<Bullet>().poolType;
    }


    public bool CanSeeTarget(Transform target)
    {
        Vector2 targetPosition = target.GetComponent<Collider2D>().bounds.center;
        RaycastHit2D hit = Physics2D.Linecast(firePoint.position, targetPosition);
       if(hit.collider && hit.collider.gameObject == target.gameObject)
        {
                return true;
        }
        return false;
    }

    public void Shoot(GameObject shooter)
    {
        Bullet bullet = GetBullet();
        bullet.SetShooter(shooter);
        bullet.SetDamage(shooter);
        Rigidbody2D rb = bullet.rb;
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    private Bullet GetBullet()
    {
        Bullet bullet;
        GameObject go;
        if (PoolManager.Instance.GetPool(poolType).usingPool)
        {
            bullet = PoolManager.Instance.GetPool(poolType).Get().GetComponent<Bullet>();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
        }
        else
        {
            go = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet = go.GetComponent<Bullet>();
        }
        return bullet;
    }


}
