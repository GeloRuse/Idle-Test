using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyBase
{
    public override void Attack()
    {
        Debug.Log("Enemy shoots you!");
        currentState = EnemyState.Attacking;
    }
}
