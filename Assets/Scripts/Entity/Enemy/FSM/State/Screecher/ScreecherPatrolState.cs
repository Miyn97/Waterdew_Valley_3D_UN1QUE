using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreecherPatrolState : IEnemyState
{
    private ScreecherController screecher;
    private Vector3 targetPosition;
    private float patrolSpeed;
    private float patrolRadius;
    private float minY = 15f;  // 고도 최소값
    private float maxY = 30f;  // 고도 최대값

    public ScreecherPatrolState(ScreecherController screecher)
    {
        this.screecher = screecher;
        patrolSpeed = screecher.moveSpeed * 0.7f;
        patrolRadius = screecher.patrolRadius;
    }

    public void Enter(EnemyController enemy)
    {
        SetRandomTarget();
        Debug.Log("Screecher: Patrol 상태 진입");
    }

    public void Update(EnemyController enemy)
    {
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

        // 플레이어 감지되면 공격 상태로 전환
        Transform target = enemy.GetTarget();
        if (target != null)
        {
            enemy.ChangeState(new ScreecherFlyAttackState(screecher, target));
        }
    }

    public void Exit(EnemyController enemy)
    {
        Debug.Log("Screecher: Patrol 상태 종료");
    }

    private void SetRandomTarget()
    {
        Vector2 offset = Random.insideUnitCircle * patrolRadius;
        Vector3 current = screecher.transform.position;
        float clampedY = Mathf.Clamp(current.y, minY, maxY); // 고도 제한
        targetPosition = new Vector3(current.x + offset.x, clampedY, current.z + offset.y);
    }
}
