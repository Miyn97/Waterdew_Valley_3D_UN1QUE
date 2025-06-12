using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class EnemyController : MonoBehaviour
{
    protected IEnemyState currentState;

    [Header("Stats")]
    public float health = 100f;
    public float moveSpeed = 3f;
    public float attackRange = 0.5f;

    [Header("Patrol")]
    public float patrolRadius = 10f;

    [Header("Detection")]
    public float detectionRange = 10f;

    public Transform player;
    public Transform raft;

    public bool isPlayerInWater;

    public bool isDead;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        raft = GameObject.FindGameObjectWithTag("Raft").transform;

        ChangeState(new EnemyIdleState(this));
    }

    protected virtual void Update()
    {
        if (isDead) return;
        currentState.Update(this);
    }

    public void ChangeState(IEnemyState state)
    {
        if (currentState != null)
            currentState.Exit(this);
        currentState = state;
        currentState.Enter(this);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0f)
            Die();
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} 사망");
        Destroy(gameObject);
    }

    public abstract void Attack(Transform target);

    private void OnDrawGizmosSelected() // 감지 범위 시각화(나중에 제거)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public Transform GetTarget()
    {
        float playerDist = Vector3.Distance(transform.position, player.position);
        float raftDist = Vector3.Distance(transform.position, raft.position);

        bool playerInRange = playerDist <= detectionRange;
        bool raftInRange = raftDist <= detectionRange;

        if (playerInRange && raftInRange)
        {
            return isPlayerInWater ? player : raft;
        }
        if (playerInRange) return player;
        if (raftInRange) return raft;

        return null;
    }
}
