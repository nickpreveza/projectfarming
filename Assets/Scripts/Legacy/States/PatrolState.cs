using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HempStateMachine;
using System;
using UnityEngine.AI;

public class PatrolState : BaseState
{
    public Vector2 patrolPosition;
    private float patrolRadius;
    Enemy owner;
    NavMeshAgent agent;
    public PatrolState(Enemy owner, NavMeshAgent agent, float patrolRadius)
    {
        this.owner = owner;
        this.agent = agent;
        this.patrolRadius = patrolRadius;
    }
    public override void Enter()
    {
        patrolPosition = GetValidPatrolPoint();
        owner.LookForNewPath(patrolPosition);

    }

    public override Type Update()
    {
        owner.UpdateTarget();

        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            patrolPosition = GetValidPatrolPoint();
            owner.LookForNewPath(patrolPosition);
        }

        return CheckSwitchState();
    }

    public override void Exit()
    {
    }

    public void SetPatrolPosition(Vector2 position)
    {
        patrolPosition = position;
    }

    protected override Type CheckSwitchState()
    {
        if (owner.IsDead)
            return typeof(DeathState);
        if (owner.currentTarget == null)
            return null; //keep patrolling if no target is found

        if (owner is OilBoss)
            return typeof(SwarmState);
        else
            return typeof(ChaseState);

    }

    private Vector2 GetValidPatrolPoint()
    {
        if (owner is OilMonster monster && monster.spawnLocation != null)
        {
            return MathHelp.GetRandomPointOnCircleCircumference(patrolRadius) + (Vector2)monster.spawnLocation.transform.position;
        }
        else
            return MathHelp.GetRandomPointOnCircleCircumference(patrolRadius) + (Vector2)owner.transform.position;
    }
}
