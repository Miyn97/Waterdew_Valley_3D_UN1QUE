using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreecherFlyAttackState : IEnemyState
{
    private ScreecherController screecher;
    private Transform target;
    private Vector3 ascendPosition;

    public ScreecherFlyAttackState(ScreecherController screecher, Transform target)
    {
        this.screecher = screecher;
        this.target = target;
    }

    public void Enter(EnemyController enemy)
    {
        if (!screecher.HasRock())
            screecher.PickUpRock();

        ascendPosition = target.position + Vector3.up * 12f;
        Debug.Log("타겟 위로 상승 중");
    }

    public void Update(EnemyController enemy)
    {
        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position,
            ascendPosition,
            enemy.moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(enemy.transform.position, ascendPosition) < 1f)
        {
            enemy.ChangeState(new ScreecherHoverAttackState(screecher, target));
        }
    }

    public void Exit(EnemyController enemy) { }
}
