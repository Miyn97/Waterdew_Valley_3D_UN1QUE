using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkController : EnemyController
{
    public override void Attack(Transform target)
    {
        if (target.CompareTag("Player"))
        {
            //target.GetComponent<PlayerHealth>()?.TakeDamage(20f);
        }
        else if (target.CompareTag("Raft"))
        {
            //target.GetComponent<RaftHealth>()?.TakeDamage(10f);
        }
    }
}
