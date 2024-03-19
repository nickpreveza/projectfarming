using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HempStateMachine;
using System;

public class CricketQueen : Enemy
{
    private float maxJumpDistance = 1.5f;
    private float minJumpDistance = 0.25f;
    private float attackJumpTime = 5;
    private float startTimeBetweenAttackJumps = 5f;
    private float timeBetweenAttackJumps;
    private float chaseJumpTime = 1f;
    private float startTimeBetweenChaseJumps = 1f;
    private float timeBetweenChaseJumps;
    public bool hasAttackJumped { get; private set; }
    public bool hasChaseJumped { get; private set; }
    public Vector2 jumpAttackPosition { get; private set; }
    public Vector2 jumpChasePosition { get; private set; }
    Vector3[] waypoints;
    private Vector2 jumpDirection;
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
        timeBetweenAttackJumps = startTimeBetweenAttackJumps;
        timeBetweenChaseJumps = startTimeBetweenChaseJumps;
        agent.speed = 0f;
        hasAttackJumped = false;
        hasChaseJumped = false;
    }

    protected override void Awake()
    {
        base.Awake();
        InitStateMachine(enemyData.attackType);
    }

    private void Update()
    {
        //used so that the attack jump can only be done a once every timeBetweenAttackJumps
        if (hasAttackJumped)
        {
            if (timeBetweenAttackJumps <= 0f)
            {
                timeBetweenAttackJumps = startTimeBetweenAttackJumps;
                hasAttackJumped = false;
            }
            else
            {
                timeBetweenAttackJumps -= Time.deltaTime;
            }
        }

        if (hasChaseJumped)
        {
            if (timeBetweenChaseJumps <= 0f)
            {
                timeBetweenChaseJumps = startTimeBetweenChaseJumps;
                hasChaseJumped = false;
            }
            else
            {
                timeBetweenChaseJumps -= Time.deltaTime;
            }
        }
    }

    protected override void InitStateMachine(EnemyData.AttackType attackType)
    {
        if (attackType == EnemyData.AttackType.JumpAttack)
        {
            var states = new Dictionary<Type, BaseState>()
            {
                { typeof(IdleState), new IdleState(this) },
                { typeof(JumpChaseState), new JumpChaseState(this, agent, chaseJumpTime) },
                { typeof(AttackState), new AttackState(this, agent, enemyData.attackTime) },
                { typeof(JumpAttackState), new JumpAttackState(this, agent, attackJumpTime) },
                { typeof(DeathState), new DeathState(this, this.GetComponent<AnimationFlag>()) }
            };
            GetComponent<StateMachine>().SetupStateMachine(states, updateRate);
        }
    }

    public void SetHasAttackJumped(bool hasAttackJumped)
    {
        this.hasAttackJumped = hasAttackJumped;
    }

    public void SetHasChaseJumped(bool hasChaseJumped)
    {
        this.hasChaseJumped = hasChaseJumped;
    }
    public bool IsWithinJumpAttackRadius(Transform target)
    {
        distanceToTarget = Vector2.Distance(target.position, transform.position);
        if (distanceToTarget > minJumpDistance && distanceToTarget <= maxJumpDistance)
        {
            return true;
        }
        return false;
    }

    public void UpdateNextJumpPoint()
    {
        if (agent.hasPath)
        {
            waypoints = agent.path.corners;
            if (waypoints.Length > 1)
            {
                float waypointDistance = Vector2.Distance(waypoints[0], waypoints[1]);
                //Vector2 direction = waypoints[1] - waypoints[0];

                if (maxJumpDistance < waypointDistance)
                {
                    jumpDirection = MathHelp.GetDistanceVector(waypoints[0], waypoints[1], maxJumpDistance);
                    //jumpDirection = direction.normalized * maxJumpDistance;
                    jumpChasePosition = (Vector2)transform.position + jumpDirection; //warp to this position
                    agent.ResetPath();
                    LookForNewPath(currentTarget);
                    return;
                }
                else if (maxJumpDistance > waypointDistance && minJumpDistance < waypointDistance)
                {
                    //jumpDirection = MathHelp.GetDirectionVector(waypoints[0], waypoints[1], waypointDistance);
                    //jumpDirection = direction.normalized * waypointDistance;
                    jumpChasePosition = waypoints[1]; //or warp to this position
                    agent.ResetPath();
                    LookForNewPath(currentTarget);
                    return;
                }
            }
            //ChasteState?
        }
    }
    public void SetJumpAttackPosition(Vector2 position)
    {
        jumpAttackPosition = position;
    }

    public bool Warp(Vector2 position)
    {
        return agent.Warp(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet" && gameObject.activeSelf)
        {
            Health--;
            Aggressiveness += 5;
        }
    }

    public void OnJumpAttackCollision()
    {
        if (gameObject.activeSelf)
        {
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            Collider2D[] colliders = Physics2D.OverlapCapsuleAll(transform.position, collider.bounds.size, CapsuleDirection2D.Horizontal, 0f);
            foreach (Collider2D col in colliders)
            {
                if (col.gameObject.tag == "Player")
                {
                    player.Health -= enemyData.attackDamage;
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(jumpAttackPosition, new Vector3(0.25f, 0.25f, 1f));

    //    Gizmos.color = Color.blue;
    //    foreach (Vector3 pos in waypoints)
    //    {
    //        Gizmos.DrawSphere(pos, 0.1f);
    //    }
    //    for (int i = 0; i < waypoints.Length; i++)
    //    {
    //        if (i + 1 < waypoints.Length)
    //        {
    //            Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
    //        }
    //    }
    //    if (waypoints.Length > 1)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawSphere(waypoints[1], 0.25f);
    //    }

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(jumpChasePosition, transform.position);
    //}
}
