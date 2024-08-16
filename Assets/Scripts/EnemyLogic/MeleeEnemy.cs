using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    public override void Attack()
    {
        Debug.Log("Enemy hits you!");
        currentState = EnemyState.Attacking;
    }
}
