using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsController : MonoBehaviour
{
    [Header("Head")]
    [SerializeField] SpriteRenderer headSprite;
    [SerializeField] Sprite headRight;
    [SerializeField] Sprite headDown;
    [SerializeField] Sprite headUp;

    [Header("Body")]
    [SerializeField] SpriteRenderer bodySprite;
    [SerializeField] Sprite bodyRight;
    [SerializeField] Sprite bodyDown;
    [SerializeField] Sprite bodyUp;

    [Header("Animation")]
    public Animator playerAnimator;
    [SerializeField] Animator effectAnimator; 
    [Space(10)]
    [Header("Expressions")]

    [SerializeField] Sprite swirlRight;
    [SerializeField] Sprite swirlDown;
    [SerializeField] Sprite swirlUp;
    [Space(5)]
    [SerializeField] Sprite happyRight;
    [SerializeField] Sprite happyDown;
    [SerializeField] Sprite happyUp;
    [Space(5)]
    [SerializeField] Sprite sadRight;
    [SerializeField] Sprite sadDown;
    [SerializeField] Sprite sadUp;
    [Space(5)]
    [SerializeField] Sprite hitRight;
    [SerializeField] Sprite hitDown;
    [SerializeField] Sprite hitUp;

    IEnumerator statusChangeCoroutine;

    [Header("Hands")]
    [SerializeField] GameObject extraHands;
    Animator extraHandsAnim;
    [SerializeField] Vector3 rightRotation;
    [SerializeField] Vector3 leftRotation;

    private void Start()
    {
        playerAnimator.SetBool("isWalking", false);
        extraHandsAnim = extraHands.GetComponent<Animator>();

        foreach (Transform child in effectAnimator.transform)
        {
            child.gameObject.SetActive(false);
        }
    }


    public void EffectHealthUp()
    {
        effectAnimator.SetTrigger("HealthUp");
    }

    public void EffectShield()
    {
        effectAnimator.SetTrigger("Shield");
    }

   

  
}



