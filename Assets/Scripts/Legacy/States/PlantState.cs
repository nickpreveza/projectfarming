using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HempStateMachine;
using System;
using UnityEngine.AI;

public class PlantState : BaseState
{
    Enemy owner;
    private float placementTimer;
    private float placementTimerStart;
    public PlantState(Enemy owner, float placementTime)
    {
        this.owner = owner;
        placementTimerStart = placementTime;
    }

    public override void Enter()
    {
        placementTimer = placementTimerStart;
    }

    public override Type Update()
    {
        placementTimer -= Time.deltaTime;
        if (placementTimer <= 0 && owner is OilBoss oilBoss)
        {
            oilBoss.PlaceOilWell();
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
        if (owner is OilBoss && owner.currentTarget == null)
            return typeof(PatrolState);
        else if (owner.currentTarget == null)
            return typeof(IdleState);

        if (placementTimer <= 0 && owner is OilBoss)
            return typeof(SwarmState);

        return null;
    }
}
