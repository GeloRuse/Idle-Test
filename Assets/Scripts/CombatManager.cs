using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField]
    private CombatUI combatUI;

    [SerializeField]
    private EnemyBase[] enemies;

    [SerializeField]
    private PlayerScript player;
    [SerializeField]
    private EnemyBase enemy;

    [SerializeField]
    private Transform playerSpawn;
    [SerializeField]
    private Transform enemySpawn;

    private Coroutine playerAttackCoroutine;
    private Coroutine enemyAttackCoroutine;

    private float currentPlayerCounter;

    private void Start()
    {
        PlayerScript.IsDead += EndCombat;
        PlayerScript.IsSwitching += WeaponSwitch;
        EnemyBase.IsDead += GetRandomEnemy;
    }

    public void InitCombat()
    {
        combatUI.Show(true);

        GetRandomEnemy();
        player.MovePlayer(playerSpawn);

        InitiatePlayerAttack();
        InitiateEnemyAttack();

        combatUI.UpdatePlayerHP(player.CurrentHealthRatio);
        combatUI.UpdateEnemyHP(enemy.CurrentHealthRatio);
    }

    public void EndCombat()
    {
        StopAllCoroutines();
        playerAttackCoroutine = null;
        enemyAttackCoroutine = null;

        Destroy(enemy.gameObject);
        combatUI.Show(false);
    }

    private void GetRandomEnemy()
    {
        combatUI.StopEnemyCounters();

        if (enemy)
            Destroy(enemy.gameObject);

        int rnd = Random.Range(1, 100);
        float curMin = 100;
        int curi = 0;
        for (int i = 0; i <= enemies.Length - 1; i++)
        {
            if (rnd < enemies[i].GetStats.spawnChance && enemies[i].GetStats.spawnChance < curMin)
            {
                curi = i;
                curMin = enemies[i].GetStats.spawnChance;
            }
        }

        enemy = Instantiate(enemies[curi], enemySpawn);
    }

    public void RequestPlayerWeaponSwitch()
    {
        player.HandleWeaponSwitch();
    }

    private void WeaponSwitch()
    {
        combatUI.StartPlayerSwitchCounter(player.GetWeaponSwitchTime);
    }

    private void InitiatePlayerAttack()
    {
        if (playerAttackCoroutine == null)
        {
            playerAttackCoroutine = StartCoroutine(PlayerAttack());
        }
    }

    private void InitiateEnemyAttack()
    {
        if (enemyAttackCoroutine == null)
        {
            enemyAttackCoroutine = StartCoroutine(EnemyAttack());
        }
    }

    private IEnumerator PlayerAttack()
    {
        currentPlayerCounter = player.GetStats.attackInterval;
        while (enemy.GetState != EnemyState.Dead || player.GetState != PlayerState.Dead)
        {
            player.Prepare();
            while (currentPlayerCounter > 0)
            {
                while (player.GetState == PlayerState.Switching)
                {
                    //currentPlayerCounter += Time.deltaTime;
                    combatUI.ShowPlayerPrepareCounter(false);
                    yield return null;
                }
                combatUI.ShowPlayerPrepareCounter(true);
                currentPlayerCounter -= Time.deltaTime;
                combatUI.PlayerPrepareCounter(currentPlayerCounter / player.GetStats.attackInterval);
                yield return null;
            }
            combatUI.ShowPlayerPrepareCounter(false);

            player.Attack();
            combatUI.StartPlayerAttackCounter(player.GetWeaponSpeed);
            yield return new WaitForSeconds(player.GetWeaponSpeed);
            enemy.TakeDamage(player.GetStats.attackDamage);
            combatUI.UpdateEnemyHP(enemy.CurrentHealthRatio);

            currentPlayerCounter = player.GetStats.attackInterval;
        }

        playerAttackCoroutine = null;
    }

    private IEnumerator EnemyAttack()
    {
        enemy.Prepare();
        combatUI.StartEnemyPrepareCounter(enemy.GetStats.attackInterval);
        yield return new WaitForSeconds(enemy.GetStats.attackInterval);
        while (enemy.GetState != EnemyState.Dead || player.GetState != PlayerState.Dead)
        {
            enemy.Attack();
            combatUI.StartEnemyAttackCounter(enemy.GetStats.attackInterval / 2);
            yield return new WaitForSeconds(enemy.GetStats.attackInterval / 2);
            player.TakeDamage(enemy.GetStats.attackDamage);
            combatUI.UpdatePlayerHP(player.CurrentHealthRatio);

            enemy.Prepare();
            combatUI.StartEnemyPrepareCounter(enemy.GetStats.attackInterval);
            yield return new WaitForSeconds(enemy.GetStats.attackInterval);
        }

        enemyAttackCoroutine = null;
    }
}
