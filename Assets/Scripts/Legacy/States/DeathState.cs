using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HempStateMachine;
using System;

public class DeathState : BaseState
{
    Enemy owner;
    AnimationFlag flag;
    
    public DeathState(Enemy owner, AnimationFlag animationFlag)
    {
        this.owner = owner;
        if (animationFlag)
            this.flag = animationFlag;
        else
            Debug.LogError("No animationFlag added to " + owner.gameObject);
    }
    public override void Enter()
    {
        if (owner is Cricket cricket)
        {
            AudioManager.Instance.PlaySoundFX(cricket.sounds, "placeholderSound1");
            //start playing animation
        }
        flag.animationFlag = true;
    }

    public override Type Update()
    {

        
        return CheckSwitchState();
    }

    public override void Exit()
    {
        Debug.Log(owner.gameObject.name + " was killed");
        //if (owner is Cricket cricket)
        //{
        //    AudioManager.Instance.Stop(cricket.sounds, "placeholderSound1");
        //    owner.Killed();
        //}
        flag.animationFlag = false;
        owner.Killed();

    }

    protected override Type CheckSwitchState()
    {
        //animation (use animationflag?) stopped playing then exit to idle? or just Exit?
        //set dead to false after respawn?
        //if (flag.animationFlag)
        //{
        //    return typeof(IdleState);
        //}
        //return null;
        return typeof(IdleState);
    }
}
