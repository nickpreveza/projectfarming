using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using HempStateMachine;
public class ChaseState : BaseState
{
    private Enemy owner;
    private NavMeshAgent agent;

    public ChaseState(Enemy owner, NavMeshAgent agent)
    {
        this.owner = owner;
        this.agent = agent;
    }
    public override void Enter()
    {
        //Debug.Log("Enter Chase state");
        agent.enabled = true;
        owner.LookForNewPath(owner.currentTarget);
    }

    public override Type Update()
    {
        owner.UpdateTarget();
        if(owner.currentTarget)
        {
            owner.LookForNewPath(owner.currentTarget);
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

        if (owner is Cricket cricket)
        {
            if (cricket.IsWithinSwarmRadius(owner.currentTarget))
                return typeof(SwarmState);
        }
        else if (owner is OilMonster monster)
        {
            if (monster.IsInMeleeAttackDistance(owner.currentTarget))
                return typeof(AttackState);
        }
        
        return null;
    }
}
