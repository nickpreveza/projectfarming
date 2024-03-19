using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public DialogueTrigger trigger;
    public GameObject manager;

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(5);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (manager.GetComponent<DialogueManager>().hasFinishedConvo)
        {
            StartCoroutine(Cooldown());
            return;
        }
        else if (collision.gameObject.CompareTag("Player") && manager.GetComponent<DialogueManager>().isActive == false)
        {
            trigger.StartDialogue();

        }
        else
        {
            return;
        }
    }
  
}
