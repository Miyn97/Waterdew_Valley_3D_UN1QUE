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
    public float attackRange = 2f;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public bool useRandomPatrol = false;
    public bool useRandomMove = false;
    public int patrolIndex = 0;

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
}
