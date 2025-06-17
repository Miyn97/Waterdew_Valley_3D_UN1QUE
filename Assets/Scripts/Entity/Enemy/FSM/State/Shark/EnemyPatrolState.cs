using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPatrolState : IEnemyState
{
    private EnemyController enemy;
    private Vector3 targetPosition;
    private float patrolRadius = 10f;
    private float patrolSpeed;

    public EnemyPatrolState(EnemyController enemy)
    {
        this.enemy = enemy;
        patrolSpeed = enemy.moveSpeed * 0.7f;
    }

    public void Enter(EnemyController enemy)
    {
        SetRandomTarget();
        Debug.Log("Patrol 상태 진입: 랜덤 위치로 이동");
    }

    public void Update(EnemyController enemy)
    {
        // 회전 처리
        Vector3 direction = (targetPosition - enemy.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(
                enemy.transform.rotation,
                targetRotation,
                5f * Time.deltaTime
            );
        }

        // 이동
        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position,
            targetPosition,
            patrolSpeed * Time.deltaTime
        );

        float dist = Vector3.Distance(enemy.transform.position, targetPosition);
        if (dist < 0.5f)
        {
            SetRandomTarget();
        }

        Transform target = enemy.GetTarget();
        if (target != null)
        {
            enemy.ChangeState(new EnemyChaseState(enemy));
        }
    }

    public void Exit(EnemyController enemy)
    {
        Debug.Log("Patrol 상태 종료");
    }

    private void SetRandomTarget()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        Vector3 origin = enemy.transform.position;
        targetPosition = new Vector3(origin.x + randomOffset.x, origin.y, origin.z + randomOffset.y);
    }
}
