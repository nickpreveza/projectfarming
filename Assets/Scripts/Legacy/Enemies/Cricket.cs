using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using HempStateMachine;

public class Cricket : Enemy
{
    private float swarmRadius = 2f;
    private float swarmTime = 4f;
    public float Health
    {
        set
        {
            health = value;

            if (health <= 0f)
            {
                IsDead = true;
            }
        }
        get
        {
            return health;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Awake()
    {
        base.Awake();
        InitStateMachine(enemyData.attackType);

    }

    protected override void InitStateMachine(EnemyData.AttackType attackType)
    {
        if (attackType == EnemyData.AttackType.SwarmAttack)
        {
            var states = new Dictionary<Type, BaseState>()
            {
                { typeof(IdleState), new IdleState(this) },
                { typeof(ChaseState), new ChaseState(this, agent) },
                { typeof(AttackState), new AttackState(this, agent, enemyData.attackTime) },
                { typeof(SwarmState), new SwarmState(this, agent, swarmRadius , swarmTime) },
                { typeof(DeathState), new DeathState(this, this.GetComponent<AnimationFlag>()) }
            };
            GetComponent<StateMachine>().SetupStateMachine(states, updateRate);
        }
    }

    public bool IsWithinSwarmRadius(Transform target)
    {
        distanceToTarget = Vector2.Distance(target.position, transform.position);
        if (distanceToTarget <= swarmRadius)
        {
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "PlayerBullet" && gameObject.activeSelf)
        {
            Health--;
            Aggressiveness++;
        }

    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    if (stateMachine.currentState is SwarmState swarmState)
    //    {
    //        List<Vector3> swarmPositions = swarmState.swarmPositions;
    //        foreach (Vector3 pos in swarmPositions)
    //        {
    //            Gizmos.DrawCube(pos, new Vector3(0.25f, 0.25f, 1f));
    //        }
    //    }
    //}
    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    if (stateMachine.currentState is SwarmState state)
    //    {
    //        Gizmos.DrawCube(state.swarmPosition, new Vector3(0.1f, 0.1f, 1f));
    //        Gizmos.DrawWireSphere(transform.position, swarmRadius);
    //    }

    //}
}
