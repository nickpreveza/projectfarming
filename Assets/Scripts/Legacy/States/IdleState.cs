using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HempStateMachine;
using System;

public class IdleState : BaseState
{
    Enemy owner;
    public IdleState(Enemy owner)
    {
        this.owner = owner;

    }

    public override Type Update()
    {
        owner.UpdateTarget();
        return CheckSwitchState();
    }
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    protected override Type CheckSwitchState()
    {
        if (owner.IsDead)
            return typeof(DeathState);
        if (owner.currentTarget == null)
        {
            if (owner is OilMonster)
                return typeof(PatrolState);
            return null;
        }
        if (owner is Cricket)
            return typeof(ChaseState);
        else if (owner is CricketQueen)
            return typeof(JumpChaseState);
        else if (owner is OilBoss oilBoss)
        {
            if (oilBoss.ShouldFlee())
                return typeof(FleeState);
            else
                return typeof(PatrolState);
        }
        else if (owner is OilMonster)
            return typeof(ChaseState);

        return null;
    }
}
