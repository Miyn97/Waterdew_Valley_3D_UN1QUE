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
        currentTarget = enemy.GetTarget();
        Debug.Log($"Chase 상태 진입 → 초기 타겟: {currentTarget?.name}");
    }

    public void Update(EnemyController enemy)
    {
        Transform newTarget = enemy.GetTarget();

        if (newTarget == null)
        {
            enemy.ChangeState(new EnemyIdleState(enemy));
            return;
        }

        if (newTarget != currentTarget)
        {
            currentTarget = newTarget;
            Debug.Log($"Chase 타겟 변경 → {currentTarget.name}");
        }

        float distance = Vector3.Distance(enemy.transform.position, currentTarget.position);
        if (distance > enemy.detectionRange * 1.2f)
        {
            enemy.ChangeState(new EnemyIdleState(enemy));
            return;
        }

        // 이동 타겟 설정
        Vector3 destination = currentTarget.position;

        // 뗏목이면 가장 가까운 모서리로 추적
        if (currentTarget.CompareTag("Raft") && enemy is SharkController shark)
        {
            Vector3 fromPos = shark.headTransform ? shark.headTransform.position : shark.transform.position;
            destination = shark.GetClosestCornerPosition(currentTarget.position, fromPos);
        }

        // 회전 처리 (진행 방향)
        Vector3 direction = (destination - enemy.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, 5f * Time.deltaTime);
        }

        // 이동 처리
        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position,
            destination,
            enemy.moveSpeed * Time.deltaTime
        );

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
