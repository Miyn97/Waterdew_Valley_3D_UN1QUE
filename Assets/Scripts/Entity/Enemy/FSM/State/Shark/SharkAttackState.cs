using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAttackState : IEnemyState
{
    private EnemyController enemy;
    private Transform target;

    public SharkAttackState(EnemyController enemy, Transform target)
    {
        this.enemy = enemy;
        this.target = target;
    }

    public void Enter(EnemyController enemy)
    {
        Debug.Log($"Shark 공격! 대상: {target.name}");
        enemy.Attack(target);
        enemy.ChangeState(new EnemyIdleState(enemy));
    }

    public void Update(EnemyController enemy)
    {

    }

    public void Exit(EnemyController enemy)
    {
        Debug.Log("Shark 공격 종료");
    }
}
