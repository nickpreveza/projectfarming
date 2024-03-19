using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HempStateMachine;
using System;

public class AttackState : BaseState
{
    private Enemy owner;
    private NavMeshAgent agent;
    private float attackTimer;
    private float attackStartTime;
    public AttackState(Enemy owner, NavMeshAgent agent, float attackTime)
    {
        this.owner = owner;
        this.agent = agent;
        this.attackStartTime = attackTime;
    }

    public override void Enter()
    {
        agent.enabled = true;
    }
    public override Type Update()
    {
        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackStartTime;
        }
        else
        {
            attackTimer -= Time.deltaTime;
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
        
        if (owner.IsInMeleeAttackDistance(owner.currentTarget) == false)
        {
            if (owner is Cricket || owner is OilMonster)
                return typeof(ChaseState);
            else if (owner is CricketQueen)
                return typeof(JumpChaseState);
        }
        return null; //still in current state
    }

    public void Attack()
    {
        if (owner.currentTarget && owner.IsInMeleeAttackDistance(owner.currentTarget))
        {
            if (owner.currentTarget.gameObject.tag == "Player")
            {
                owner.player.Health -= owner.enemyData.attackDamage;

            }
            else if (owner.currentTarget.gameObject.tag == "HempPlant")
            {
                owner.currentTarget.GetComponent<HempGrowth>().Health--;
            }
            
        }
    }
}
