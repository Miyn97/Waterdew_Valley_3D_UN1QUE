using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private EnemyController enemy;
    private float waitTime;

    public EnemyIdleState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter(EnemyController enemy)
    {
        waitTime = Random.Range(2f, 4f);
        Debug.Log("Idle 상태 진입");
    }

    public void Update(EnemyController enemy)
    {
        waitTime -= Time.deltaTime;

        Transform target = enemy.GetTarget();
        if (target != null)
        {
            enemy.ChangeState(new EnemyChaseState(enemy));
            return;
        }

        if (waitTime <= 0f)
        {
            enemy.ChangeState(new EnemyPatrolState(enemy));
        }
    }

    public void Exit(EnemyController enemy)
    {
        Debug.Log("Idle 상태 종료");
    }
}
