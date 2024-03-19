using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFlag : MonoBehaviour
{
    public bool animationFlag { get; set; }
    public void ToggleAnimationFlagTrue()
    {
        animationFlag = true; ;
    }
}
