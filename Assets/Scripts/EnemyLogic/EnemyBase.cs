using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Preparing,
    Attacking,
    Dead
}

public abstract class EnemyBase : MonoBehaviour
{
    public static event Action IsDead;

    [SerializeField]
    protected EnemyStats enemyStats;
    [SerializeField]
    protected int currentHealth;
    [SerializeField]
    protected EnemyState currentState;

    public EnemyState GetState => currentState;
    public EnemyStats GetStats => enemyStats;
    public float CurrentHealthRatio => (float)currentHealth / enemyStats.maxHealth;

    protected virtual void Awake()
    {
        InitStats();
    }

    private void InitStats()
    {
        if (!enemyStats)
        {
            Debug.Log("NULL ENEMY STATS!");
            return;
        }

        currentHealth = enemyStats.maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            currentState = EnemyState.Dead;
            IsDead?.Invoke();
        }
    }

    public virtual void Prepare()
    {
        currentState = EnemyState.Preparing;
    }

    public abstract void Attack();
}
