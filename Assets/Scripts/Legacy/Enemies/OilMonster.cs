using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HempStateMachine;

public class OilMonster : Enemy
{
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

    protected override void Awake()
    {

        base.Awake();
        //SetEnemyType(EnemyData.EnemyType.OilMonster);
        InitStateMachine(enemyData.attackType);

    }
    protected override void OnEnable()
    {
        base.OnEnable();

    }
    protected override void InitStateMachine(EnemyData.AttackType attackType)
    {
        if (attackType == EnemyData.AttackType.Melee)
        {
            var states = new Dictionary<Type, BaseState>()
            {
                { typeof(IdleState), new IdleState(this) },
                { typeof(PatrolState), new PatrolState(this, agent, detectionRadius) },
                { typeof(ChaseState), new ChaseState(this, agent) },
                { typeof(AttackState), new AttackState(this, agent, enemyData.attackTime) },
                { typeof(DeathState), new DeathState(this, this.GetComponent<AnimationFlag>()) }

            };
            GetComponent<StateMachine>().SetupStateMachine(states, updateRate);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet" && gameObject.activeSelf)
        {
            Health--;
            Aggressiveness++;
        }
    }
}
