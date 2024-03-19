using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFadeOut : MonoBehaviour
{
    Color spriteColor;
    float modifier = 0;
    bool fadeOut =false, fadeIn =false; 
    void Start()
    {
        spriteColor=this.gameObject.GetComponent<SpriteRenderer>().color;

    }
    private void FixedUpdate()
    {
        StartCoroutine(FadeOverTime(modifier));
        if (spriteColor.a <= 0.5f)
        {
            spriteColor.a = 0.5f;
        }
        this.gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
    }
    IEnumerator FadeOverTime(float modifier)
    {
        if (spriteColor.a <= 0.5f)
        {
            spriteColor.a = 0.5f;
        }
        else if (spriteColor.a >= 1f)
        {
            spriteColor.a = 1f;
        }
        for (int i = 0; i < 5; i++)
        {
            spriteColor.a += modifier;
            yield return new WaitForSeconds(0.15f);
        }
       

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            modifier = -0.05f;
           fadeOut = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            modifier = 0.05f;
            fadeIn = true;
        }
    }
}
