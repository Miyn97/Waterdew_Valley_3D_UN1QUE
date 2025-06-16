using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkController : EnemyController
{
    public override void Attack(Transform target)
    {
        if (target.CompareTag("Player"))
        {
            PlayerStatus status = target.GetComponent<PlayerStatus>();
            if (status != null)
            {
                status.TakeDamage(20f);
                Debug.Log("Shark가 플레이어에게 피해를 입혔습니다.");
            }
        }
        else if (target.CompareTag("Raft"))
        {
            //RaftHealth raft = target.GetComponent<RaftHealth>();
            //if (raft != null)
            //{
            //    raft.TakeDamage(10f);
            //    Debug.Log("Shark가 뗏목에 피해를 입혔습니다.");
            //}
        }
    }
}
