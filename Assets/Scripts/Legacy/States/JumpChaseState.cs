using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HempStateMachine;
using System;
using UnityEngine.AI;

public class JumpChaseState : BaseState
{
    private Enemy owner;
    private NavMeshAgent agent;
    float timer;
    float startTime;
    public JumpChaseState(Enemy owner, NavMeshAgent agent, float timeBetweenJumps)
    {
        this.owner = owner;
        this.agent = agent;
        startTime = timeBetweenJumps;
    }
    public override void Enter()
    {
        agent.enabled = true;
        timer = startTime;
        agent.ResetPath();
        owner.LookForNewPath(owner.currentTarget);
    }

    public override Type Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f && owner.currentTarget)
        {
            owner.LookForNewPath(owner.currentTarget);
        }
        //    if (agent.hasPath == false && owner.currentTarget != null)
        //{
        //    agent.ResetPath();
        //    owner.LookForNewPath(owner.currentTarget);
        //}

        if (timer <= 0)
        {
            timer = startTime;
            Jump();
        }
        else
        {
            timer -= Time.deltaTime;
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

        if (owner is CricketQueen queen && queen.IsWithinJumpAttackRadius(owner.currentTarget) && owner.currentTarget.tag == "Player" && queen.hasAttackJumped == false)
        {
            return typeof(JumpAttackState);
        }
        else if (owner.IsInMeleeAttackDistance(owner.currentTarget))
        {
            return typeof(AttackState);
        }
        return null;

    }

    private void Jump()
    {
        if (owner is CricketQueen queen)
        {
            queen.UpdateNextJumpPoint();
            queen.Warp(queen.jumpChasePosition);
            queen.SetHasChaseJumped(true);
        }
    }
}
