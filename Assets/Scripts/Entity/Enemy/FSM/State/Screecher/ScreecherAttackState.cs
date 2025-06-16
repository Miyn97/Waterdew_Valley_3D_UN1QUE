using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreecherAttackState : IEnemyState
{
    private ScreecherController screecher;
    private Transform target;

    public ScreecherAttackState(ScreecherController screecher, Transform target)
    {
        this.screecher = screecher;
        this.target = target;
    }

    public void Enter(EnemyController enemy)
    {
        Debug.Log("돌 낙하 공격");
        screecher.Attack(target);
    }

    public void Update(EnemyController enemy)
    {
        screecher.ChangeState(new ScreecherIdleState(screecher, 2f));
    }

    public void Exit(EnemyController enemy) { }
}
