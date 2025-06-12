using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private EnemyController enemy;
    private Transform currentTarget;

    public EnemyChaseState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter(EnemyController enemy)
    {
        Debug.Log($"Chase 상태 진입");
    }

    public void Update(EnemyController enemy)
    {
        Transform newTarget = enemy.isPlayerInWater ? enemy.player : enemy.raft;

        if (newTarget != currentTarget)
        {
            currentTarget = newTarget;
            Debug.Log($"[Chase] 타겟 변경: {currentTarget.name}");
        }

        if (currentTarget == null) return;

        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position,
            currentTarget.position,
            enemy.moveSpeed * Time.deltaTime
        );

        float distance = Vector3.Distance(enemy.transform.position, currentTarget.position);
        if (distance <= enemy.attackRange)
        {
            enemy.ChangeState(new SharkAttackState(enemy, currentTarget));
        }
    }

    public void Exit(EnemyController enemy)
    {
        Debug.Log("Chase 상태 종료");
    }
}
