using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreecherHoverAttackState : IEnemyState
{
    private ScreecherController screecher;
    private Transform target;
    private float hoverTime = 2f;
    private float timer;

    public ScreecherHoverAttackState(ScreecherController screecher, Transform target)
    {
        this.screecher = screecher;
        this.target = target;
    }

    public void Enter(EnemyController enemy)
    {
        timer = hoverTime;
        Debug.Log("목표 위 공중 대기");
    }

    public void Update(EnemyController enemy)
    {
        if (target == null)
        {
            enemy.ChangeState(new ScreecherPatrolState(screecher));
            return;
        }

        enemy.transform.position = Vector3.Lerp(
            enemy.transform.position,
            target.position + Vector3.up * 10f,
            Time.deltaTime
        );

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            enemy.ChangeState(new ScreecherAttackState(screecher, target));
        }
    }

    public void Exit(EnemyController enemy)
    {
        Debug.Log("돌 낙하 준비");
    }
}
