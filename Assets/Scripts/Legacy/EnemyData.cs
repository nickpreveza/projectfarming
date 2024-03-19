using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public enum AttackType
    {
        Melee,
        Ranged,
        SwarmAttack,
        JumpAttack
    };

    public enum EnemyType
    {
        Cricket,
        CricketQueen,
        OilMonster,
        OilBoss
    }

    public bool isBoss = false;
    [Tooltip("this is used by the WaveManager. lower number mean it can afford to spawn more of this type")]
    public float maxHealth = 5;
    public float movementSpeed = 5f;
    public float detectionRadius = 5f;
    public AttackType attackType = AttackType.Melee;
    public EnemyType enemyType = EnemyType.Cricket;
    [Header("Aggressivness towards the player")]
    [Tooltip("Higher number will lead to changing target to player more common")]
    [Range(-100, 100)]
    public int aggressiveness = 0;
    [Header("Range from the target it will start Melee attacking")] 
    public float attackingRange = 1f;
    [Header("Damage per attack")] 
    public float attackDamage = 1f;
    [Header("AttackTime")]
    [Range(0.1f, 15f)]
    public float attackTime = 1f;
}
