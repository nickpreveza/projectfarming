using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelay : MonoBehaviour
{
    Animator anim;

    private IEnumerator Start()
    {
        anim= GetComponent<Animator>();
        anim.speed = 0;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.9f));
        anim.speed = Random.Range(0.95f, 1.5f);
    }
}
