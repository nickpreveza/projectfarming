using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWeaponMelee : MonoBehaviour
{
    Entity handler;
    public bool haveAppliedDamage;
    [SerializeField] bool hasBeenSetup;
    public void Setup(Entity _handler)
    {
        handler = _handler;
        hasBeenSetup = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasBeenSetup)
        {
            return;
        }
        if (collision.CompareTag("Player") && !haveAppliedDamage)
        {
            HB_PlayerController.Instance.OnDamage(handler.attackDamage);
            haveAppliedDamage = true;
        }
    }
}
