using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HempStateMachine;
using UnityEngine.AI;
using System;

public class FleeState : BaseState
{
    Enemy owner;
    NavMeshAgent agent;
    float fleeingRange;
    float distanceToTarget;
    Vector2 fleeingVector;
    public FleeState(Enemy owner, NavMeshAgent agent, float fleeingRange)
    {
        this.owner = owner;
        this.agent = agent;
        this.fleeingRange = fleeingRange;
    }

    public override void Enter()
    {
        agent.speed *= 3;
        if (owner.currentTarget)
        {
            distanceToTarget = Vector2.Distance(owner.transform.position, owner.currentTarget.position);
            if (distanceToTarget <= fleeingRange)
            {
                SetFleeingVector();
            }
        }
    }


    public override Type Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f && owner.currentTarget)
        {
            distanceToTarget = Vector2.Distance(owner.transform.position, owner.currentTarget.position);
            if (distanceToTarget <= fleeingRange)
            {
                SetFleeingVector();
            }
        }

        owner.LookForNewPath(fleeingVector);
        return CheckSwitchState();
    }

    public override void Exit()
    {
        agent.speed /= 3;
    }


    protected override Type CheckSwitchState()
    {
        if (owner.IsDead)
            return typeof(DeathState);
        if (owner.currentTarget == null)
            return typeof(IdleState);

        if (distanceToTarget > fleeingRange)
        {
            return typeof(PatrolState);
        }

        return null;
    }
    private void SetFleeingVector()
    {
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            //get the Vector from the target to the this gameobject to get opposite direction to flee.
            fleeingVector = (Vector2)owner.transform.position + MathHelp.GetDirectionVector(owner.currentTarget.position, owner.transform.position) * (fleeingRange * 3);
        }
        else
        {
            fleeingVector = (Vector2)owner.currentTarget.position + MathHelp.GetRandomPointOnCircleCircumference(fleeingRange * 3);

        }
    }
}
