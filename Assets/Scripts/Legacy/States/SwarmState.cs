using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HempStateMachine;
using System;

public class SwarmState : BaseState
{
    private Enemy owner;
    private NavMeshAgent agent;
    private float startRadius;
    private float swarmRadius;
    private Vector2 swarmPositionNoTarget;
    private Vector2 placementLocation;
    private float swarmTimer = 0f;
    private float startTime;

    public SwarmState(Enemy owner, NavMeshAgent agent, float swarmRadius, float swarmTime)
    {
        this.owner = owner;
        this.agent = agent;
        startRadius = swarmRadius;
        startTime = swarmTime;
    }
    public override void Enter()
    {
        swarmPositionNoTarget = Vector2.zero;
        agent.enabled = true;
        agent.autoBraking = false;
        swarmRadius = startRadius;
        swarmTimer = startTime;
        if (owner is OilBoss oilBoss)
        {
            oilBoss.SetRandomClockwiseMovement();
            placementLocation = GetSwarmPosition(oilBoss, 45f);
            owner.LookForNewPath(placementLocation);
        }
        else
            owner.LookForNewPath(GetRandomSwarmPosition());
    }
    public override Type Update()
    {
        if (owner is Cricket && swarmTimer > 0f)
        {
            Swarm();
        }
        swarmTimer -= Time.deltaTime;
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
        if (owner is OilBoss oilBoss)
            return UpdateOilBoss(oilBoss);

        if (owner is Cricket cricket && swarmTimer <= 0f)
        {
            if (owner.IsInMeleeAttackDistance(owner.currentTarget))
                return typeof(AttackState);
            else if(cricket.IsWithinSwarmRadius(owner.currentTarget) == false)
                return typeof(ChaseState);
            else
                owner.LookForNewPath(owner.currentTarget);
        }

        return null;
    }

    private void Swarm()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            if (swarmRadius >= 0.5f)
            {
                swarmRadius *= 0.75f;
            }
            owner.LookForNewPath(GetRandomSwarmPosition());
        }
    }
    private Type UpdateOilBoss(OilBoss oilBoss)
    {
        if (oilBoss.ShouldFlee())
        {
            return typeof(FleeState);
        }
        else if (oilBoss.HasMaxOilWells() && owner.currentTarget != null && oilBoss.CanSeeTarget())
        {
            return typeof(RangeAttackState);
        }
        else if (oilBoss.HasMaxOilWells() == false && oilBoss.CanPlaceOilWell() && agent.remainingDistance < 0.2f)
        {
            return typeof(PlantState);
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            placementLocation = GetSwarmPosition(oilBoss, 45f);
            oilBoss.LookForNewPath(placementLocation);
        }

        return null;
    }
    private Vector2 GetRandomSwarmPosition()
    {
        if (owner.currentTarget)
            return MathHelp.GetRandomPointOnCircleCircumference(swarmRadius) + (Vector2)owner.currentTarget.position;
        else
        {
            if (swarmPositionNoTarget == Vector2.zero)
            {
                swarmPositionNoTarget = owner.transform.position;
            }
            return MathHelp.GetRandomPointOnCircleCircumference(swarmRadius) + swarmPositionNoTarget;
        }
    }

    private Vector2 GetSwarmPosition(OilBoss oilboss, float angleOffset)
    {
        return oilboss.GetPlacementLocation(angleOffset);
    }


}
