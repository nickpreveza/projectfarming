using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HempStateMachine;

[RequireComponent(typeof(StateMachine))]
public class Enemy : AIBase
{
    [HideInInspector] public StateMachine stateMachine => GetComponent<StateMachine>();
    public PlayerManager player { get; private set; }
    public Transform currentTarget { get; private set; }
    [SerializeField]
    public EnemyData enemyData;
    [HideInInspector]
    protected float updateRate { get; private set; } = 0.016f * 1f; // 3 frames per second
    public Sound sounds { get; private set; }
    private List<GameObject> targetsInRange;
    //public EnemyData.EnemyType enemyType { get; private set; }
    private EnemyData.AttackType attackType;
    protected float distanceToTarget;
    protected float health;
    private int aggressiveness;
    private float touchDamage = 0.1f;
    private float damageTimer;
    private float damageTimerStart = 1;
    private bool isDead;
    public bool IsDead { get { return isDead; } protected set { isDead = value; } }
    public float detectionRadius { get; private set; }
    public EnemySpawner spawnLocation { get; private set; }
    public LayerMask preferredTarget { get; private set; }

    
    public int Aggressiveness
    {
        set
        {
            aggressiveness = value;
        }
        get
        {
            return aggressiveness;
        }
    }
    protected virtual void OnEnable()
    {
        IsDead = false;
        health = enemyData.maxHealth;
        agent.speed = enemyData.movementSpeed;
        aggressiveness = enemyData.aggressiveness;
        detectionRadius = enemyData.detectionRadius;
        agent.autoBraking = false;
        
    }

    protected virtual void Awake()
    {
        base.Init();
        sounds = GetComponent<Sound>();
        preferredTarget = LayerMask.GetMask("Hemp");
        attackType = enemyData.attackType;
        health = enemyData.maxHealth;
        agent.speed = enemyData.movementSpeed;
        aggressiveness = enemyData.aggressiveness;
        detectionRadius = enemyData.detectionRadius;
        agent.autoBraking = false;
        targetsInRange = new List<GameObject>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        }

    }
    protected override void InitStateMachine(EnemyData.AttackType attackType)
    {
    }
    //protected override void InitStateMachine(EnemyData.AttackType attackType)
    //{
    //    if (attackType == EnemyData.AttackType.Melee)
    //    {
    //        var states = new Dictionary<Type, BaseState>()
    //        {
    //            { typeof(IdleState), new IdleState(this) },
    //            { typeof(ChaseState), new ChaseState(this, agent) },
    //            { typeof(AttackState), new AttackState(this, agent) }
    //        };
    //        GetComponent<StateMachine>().SetAvailableStates(states);
    //    }
    //}

    public void SetSpawnLocation(EnemySpawner spawnLocation)
    {
        this.spawnLocation = spawnLocation;
        if (spawnLocation != null)
        {
            transform.position = this.spawnLocation.transform.position;
        }
    }

    public new void LookForNewPath(Transform target)
    {
        base.LookForNewPath(target);
    }
    public new void LookForNewPath(Vector2 position)
    {
        base.LookForNewPath(position);
    }

    public void UpdateTarget()
    {
        if (this is OilBoss)
            currentTarget = player.transform;
        else

            currentTarget = GetTarget();
    }

    protected void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    public Transform GetTarget()
    {
        targetsInRange.Clear();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, preferredTarget);
        float shortestDistance = 1000f;
        float distance = 0f;
        Transform target = null;
        foreach (Collider2D collider in colliders)
        {
            distance = Vector2.Distance(this.transform.position, collider.transform.position);
            if (shortestDistance == 1000f)
            {
                shortestDistance = distance;
                target = collider.transform;
            }
            else if (distance < shortestDistance)
            {
                shortestDistance = distance;
                target = collider.transform;
            }

        }

        if (target != null && shortestDistance <= this.detectionRadius)
        {
            target = GetTargetDependingOnAggressiveness(player.transform, target, shortestDistance);
        }
        else if (target == null)
        {
            target = GetPlayerInRange();
        }
        return target;
    }

    private Transform GetPlayerInRange()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(this.transform.position, player.transform.position);
            if (distance <= this.detectionRadius)
            {
                return player.transform;
            }
        }
        return null;
    }


    private Transform GetTargetDependingOnAggressiveness(Transform playerTarget, Transform closestTarget, float distanceToClosestTarget)
    {
        float distanceToPlayer = Vector2.Distance(playerTarget.transform.position, transform.position);
        if ((distanceToClosestTarget + Aggressiveness) / distanceToPlayer >= 10)
        {
            return playerTarget;
        }
        return closestTarget;

    }

    public bool IsInMeleeAttackDistance(Transform target)
    {
        distanceToTarget = Vector2.Distance(target.position, transform.position);
        if (distanceToTarget <= enemyData.attackingRange)
        {
            return true;
        }
        return false;
    }


    //public Type GetChaseState()
    //{
    //    if (currentTarget == null)
    //        return null;

    //    if (this is Cricket || this is OilMonster)
    //    {
    //        return typeof(ChaseState);
    //    }
    //    else if(this is CricketQueen)
    //    {
    //        return typeof(JumpChaseState);
    //    }

    //    return null;
    //}

    private Type GetAttackState()
    {
        if (IsInMeleeAttackDistance(currentTarget))
        {
            return typeof(AttackState);
        }
        else if (this is Cricket cricket && cricket.IsWithinSwarmRadius(currentTarget))
        {
            return typeof(SwarmState);
        }
        else if (this is CricketQueen queen && queen.IsWithinJumpAttackRadius(currentTarget) && currentTarget.tag == "Player" && queen.hasAttackJumped == false)
        {
            return typeof(JumpAttackState);
        }
        else
            return null;
    }
    public void Killed()
    {
        if (spawnLocation != null)
        {
            spawnLocation.Kill(this);
        }
        else
        {
            Destroy(this.gameObject, 1f);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        InflictTouchDamage();
    //        damageTimer = damageTimerStart;
    //        Aggressiveness++;
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        if(damageTimer <= 0f)
    //        {
    //            InflictTouchDamage();
    //            Aggressiveness++;
    //        }
    //        else
    //        {
    //            damageTimer -= Time.deltaTime;
    //        }
    //    }
    //}

    //private void InflictTouchDamage()
    //{
    //    player.Health -= touchDamage;
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRadius);
    //}
}
