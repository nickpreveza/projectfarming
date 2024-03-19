using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HempStateMachine;
using System;

public class OilBoss : Enemy
{
    public GameObject oilWellPrefab;
    private int maxOilWells = 4;
    private List<EnemySpawner> oilWells;
    private float minimumOilWellDistance = 1;
    //private float patrolRadius = 4f;
    private float fleeRadius;
    private float placementRadius;
    private float placementTime = 1f;
    Transform placementPosition;
    private bool circlingClockwise = false;
    Rigidbody2D rb;
    private Shooting shooting;
    private Vector2 shootingVector;
    private float shootingAngle;
    private float timeBetweenShots;
    private Collider2D playerCollider;
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
        timeBetweenShots = enemyData.attackTime;
        fleeRadius = detectionRadius * 0.5f;
        placementRadius = detectionRadius * 0.9f; //placement of oil plants distance, it has to be higher than flee distance
        oilWells = new List<EnemySpawner>();
        InitStateMachine(enemyData.attackType);
        shooting = GetComponent<Shooting>();
        if (oilWellPrefab == null || shooting == null)
        {
            Debug.LogError("Some objetcs not set to the OilBoss");
        }
        rb = GetComponent<Rigidbody2D>();
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform trans in children)
        {
            if (trans.name.Equals("PlacementPosition"))
            {
                placementPosition = trans;
            }
        }
        playerCollider = player.GetComponent<Collider2D>();



    }
    protected override void OnEnable()
    {
        base.OnEnable();
        SetRandomClockwiseMovement();

    }
    private void FixedUpdate()
    {
        shootingVector = playerCollider.bounds.center - transform.position;
        shootingAngle = MathF.Atan2(shootingVector.y, shootingVector.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = shootingAngle;
    }
    protected override void InitStateMachine(EnemyData.AttackType attackType)
    {
        if (attackType == EnemyData.AttackType.Ranged)
        {
            var states = new Dictionary<Type, BaseState>()
            {
                { typeof(IdleState), new IdleState(this) },
                { typeof(PatrolState), new PatrolState(this, agent, detectionRadius) },
                { typeof(SwarmState), new SwarmState(this, agent, placementRadius, 0f) },
                { typeof(PlantState), new PlantState(this, placementTime) },
                { typeof(RangeAttackState), new RangeAttackState(this, timeBetweenShots) },
                { typeof(FleeState), new FleeState(this, agent, fleeRadius) },
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

    public void SetRandomClockwiseMovement()
    {
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
            circlingClockwise = false;
        else
            circlingClockwise = true;
    }

    public bool HasMaxOilWells()
    {
        if (oilWells.Count >= maxOilWells)
            return true;
        return false;
    }

    public bool CanPlaceOilWell()
    {
        float distance;
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= placementRadius)
        {
            foreach (EnemySpawner spawner in oilWells)
            {
                distance = Vector2.Distance(transform.position, spawner.transform.position);
                if (distance < minimumOilWellDistance)
                {
                    return false;
                }
            }
            Collider2D collider = Physics2D.OverlapCircle(placementPosition.position, oilWellPrefab.gameObject.GetComponent<CircleCollider2D>().radius);
            if (collider == null)
            {
                return true;
            }
        }
        return false;
    }

    public void PlaceOilWell()
    {
        GameObject go = Instantiate(oilWellPrefab);
        go.transform.position = placementPosition.position;
        EnemySpawner spawner = go.GetComponent<EnemySpawner>();
        spawner.Create(this, EnemyData.EnemyType.OilMonster);
        oilWells.Add(spawner);
    }

    public Vector2 GetPlacementLocation(float angleOffset)
    {
        float angle;
        Vector2 plantLocation = Vector2.zero;
        for (int i = 0; i < 3; i++) //only try to find a spot for 3 times to avoid lag
        {
            angle = MathHelp.GetAngle(player.transform.position, transform.position);
            angle = MathHelp.GetManipulatedAngle(angle, angleOffset, circlingClockwise);
            plantLocation = MathHelp.GetPointOnCircleCircumference(placementRadius, angle) + (Vector2)player.transform.position;
        }

        return plantLocation;
    }

    public bool CanSeeTarget()
    {
        if (currentTarget)
        {
            return shooting.CanSeeTarget(currentTarget);
        }
        return false;
    }
    public bool ShouldFlee()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < fleeRadius)
        {
            SetTarget(player.transform);
            return true;
        }
        return false;
    }

    public void DestroyOilWell(EnemySpawner oilWell)
    {
        oilWells.Remove(oilWell);
        Destroy(oilWell.gameObject);
    }

    public void Shoot()
    {
        shooting.Shoot(this.gameObject);
    }


    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    if (stateMachine.currentState is PatrolState state)
    //    {
    //        Gizmos.DrawCube(state.patrolPosition, new Vector3(0.1f, 0.1f, 1f));
    //        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(transform.position, fleeRadius);
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireSphere(transform.position, placementRadius);
    //    }

    //}
}
