using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : IEnemyState
{
    private EnemyController enemy;
    private Transform targetPoint;
    private float patrolSpeed;

    public EnemyPatrolState(EnemyController enemy)
    {
        this.enemy = enemy;
        patrolSpeed = enemy.moveSpeed * 0.7f; // 순찰은 살짝 느리게
    }

    public void Enter(EnemyController enemy)
    {
        SelectNextPoint();
        Debug.Log("Enemy: Patrol 상태 진입");
    }

    public void Update(EnemyController enemy)
    {
        if (targetPoint == null)
        {
            enemy.ChangeState(new EnemyIdleState(enemy));
            return;
        }

        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position,
            targetPoint.position,
            patrolSpeed * Time.deltaTime
        );

        float dist = Vector3.Distance(enemy.transform.position, targetPoint.position);
        if (dist < 0.5f)
        {
            SelectNextPoint();
        }

        // 플레이어 발견 시 추적 상태로 전환
        if (Vector3.Distance(enemy.transform.position, enemy.player.position) < 10f)
        {
            enemy.ChangeState(new EnemyPatrolState(enemy));
        }
    }

    public void Exit(EnemyController enemy)
    {
        Debug.Log("Enemy: Patrol 상태 종료");
    }

    private void SelectNextPoint()
    {
        if (enemy.patrolPoints == null || enemy.patrolPoints.Length == 0)
        {
            Debug.LogWarning("Patrol 지점이 설정되지 않았습니다.");
            targetPoint = null;
            return;
        }

        if (enemy.useRandomPatrol)
        {
            targetPoint = enemy.patrolPoints[Random.Range(0, enemy.patrolPoints.Length)];
        }
        else
        {
            targetPoint = enemy.patrolPoints[enemy.patrolIndex % enemy.patrolPoints.Length];
            enemy.patrolIndex++;
        }
    }
}
