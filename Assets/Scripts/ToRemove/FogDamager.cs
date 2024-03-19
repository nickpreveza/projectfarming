using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogDamager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HB_PlayerController.Instance.FogStatusChanged(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HB_PlayerController.Instance.FogStatusChanged(false);
        }
    }
}
