using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxPlacement : MonoBehaviour
{
    public GameObject npcParent;

    private void Awake()
    {
        this.transform.position = npcParent.transform.position + new Vector3(0, 2,0);
    }
}
