using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreecherIdleState : IEnemyState
{
    private ScreecherController screecher;
    private float idleTime;

    public ScreecherIdleState(ScreecherController screecher, float duration = 2f)
    {
        this.screecher = screecher;
        idleTime = duration;
    }

    public void Enter(EnemyController enemy)
    {
        Debug.Log("Idle 상태 진입 (휴식)");
    }

    public void Update(EnemyController enemy)
    {
        idleTime -= Time.deltaTime;

        if (idleTime <= 0f)
        {
            Transform target = enemy.GetTarget();
            if (target != null)
            {
                enemy.ChangeState(new ScreecherFlyAttackState(screecher, target)); // 다시 공격 루프로
            }
            else
            {
                enemy.ChangeState(new ScreecherPatrolState(screecher)); // 감지 실패 -> 순찰
            }
        }
    }

    public void Exit(EnemyController enemy)
    {
        Debug.Log("Idle 상태 종료");
    }
}
