using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAttackState : IEnemyState
{
    private EnemyController enemy;
    private Transform target;

    private float attackDelay = 0.8f;
    private float elapsedTime = 0f;
    private bool hasAttacked = false;

    public SharkAttackState(EnemyController enemy, Transform target)
    {
        this.enemy = enemy;
        this.target = target;
    }

    public void Enter(EnemyController enemy)
    {
        Debug.Log($"Shark 공격 준비. 대상: {target.name}");
        elapsedTime = 0f;
        hasAttacked = false;

        // 애니메이션 추가
        // enemy.Animator.SetTrigger("Attack");
    }

    public void Update(EnemyController enemy)
    {
        elapsedTime += Time.deltaTime;

        if (!hasAttacked && elapsedTime >= attackDelay)
        {
            enemy.Attack(target);
            hasAttacked = true;
        }

        if (hasAttacked && elapsedTime >= attackDelay + 0.5f)
        {
            enemy.ChangeState(new EnemyIdleState(enemy));
        }
    }

    public void Exit(EnemyController enemy)
    {
        Debug.Log("Shark 공격 종료");
    }
}
