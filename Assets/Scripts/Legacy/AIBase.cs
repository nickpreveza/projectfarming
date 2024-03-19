using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class AIBase : MonoBehaviour
{
    protected NavMeshAgent agent => GetComponent<NavMeshAgent>();
    protected abstract void InitStateMachine(EnemyData.AttackType attackType);

    protected virtual void Init()
    {
        FixRotationBug();
    }

    /// <summary>
    /// Looks for new path instantly.
    /// </summary>
    protected virtual void LookForNewPath(Transform target)
    {
        agent.SetDestination(target.position);
    }
    protected virtual void LookForNewPath(Vector2 position)
    {
        agent.SetDestination(position);
    }


    //this is needed to prevent a bug according to the developer of NavMeshPro. It needs to be in all Start methods of all agents
    private void FixRotationBug()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
}
