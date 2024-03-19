using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HempStateMachine;
using System;

public class JumpAttackState : BaseState
{

    private Enemy owner;
    private NavMeshAgent agent;
    private float jumpTimer = 0f;
    private float startTime;
    private GameObject attackEffect;
    private SpriteRenderer effectRenderer;
    private Sprite sprite;

    public JumpAttackState(Enemy owner, NavMeshAgent agent, float attackJumpTime)
    {
        this.owner = owner;
        this.agent = agent;
        startTime = attackJumpTime;
        attackEffect = owner.gameObject.transform.GetChild(1).gameObject;
        effectRenderer = attackEffect.GetComponent<SpriteRenderer>();
        attackEffect.SetActive(false);

    }
    public override void Enter()
    {
        agent.enabled = true;
        sprite = owner.GetComponent<SpriteRenderer>().sprite;
        jumpTimer = startTime;
        JumpAttack();
    }
    public override Type Update()
    {

        if (owner is CricketQueen queen)
        {
            attackEffect.transform.position = queen.jumpAttackPosition;
        }
        jumpTimer -= Time.deltaTime;
        return CheckSwitchState();
    }

    public override void Exit()
    {
        if (owner is CricketQueen queen)
        {
            queen.GetComponent<SpriteRenderer>().sprite = sprite;
            queen.Warp(queen.jumpAttackPosition);
            queen.SetHasAttackJumped(true);
            queen.OnJumpAttackCollision();
            agent.ResetPath();
            attackEffect.transform.position = attackEffect.transform.parent.position;
            attackEffect.SetActive(false);
        }


    }

    protected override Type CheckSwitchState()
    {
        if (owner.IsDead)
            return typeof(DeathState);
        if (owner.currentTarget == null || jumpTimer <= 0)
            return typeof(IdleState);

        return null;
    }

    private void JumpAttack()
    {
        if (owner is CricketQueen queen)
        {
            if (queen.currentTarget != null)
            {
                queen.SetJumpAttackPosition(queen.currentTarget.position);
                attackEffect.SetActive(true);
                attackEffect.transform.position = queen.jumpAttackPosition;
                queen.GetComponent<SpriteRenderer>().sprite = null;
                attackEffect.GetComponent<Animator>().Play("JumpAttackShadow");
            }
        }
    }
}
