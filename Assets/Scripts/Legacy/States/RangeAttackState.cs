using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HempStateMachine;

public class RangeAttackState : BaseState
{
    Enemy owner;
    float timer;
    float startTimer;
    public RangeAttackState(Enemy owner, float timeForShot)
    {
        this.owner = owner;
        this.startTimer = timeForShot;
    }
    public override void Enter()
    {
        timer = startTimer;
    }

    public override Type Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f && owner is OilBoss oilBoss)
        {
            oilBoss.Shoot();
        }
        return CheckSwitchState();

    }

    public override void Exit()
    {

    }

    protected override Type CheckSwitchState()
    {
        if (owner.IsDead)
            return typeof(DeathState);
        if (owner.currentTarget == null)
            return typeof(IdleState);
        if (timer <= 0f && owner is OilBoss)
            return typeof(SwarmState);
        return null;
    }
}
