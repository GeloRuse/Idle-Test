using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Preparing,
    Attacking,
    Switching,
    Dead
}

public class PlayerScript : MonoBehaviour
{
    public static event Action IsDead;
    public static event Action IsSwitching;

    [SerializeField]
    private WeaponScript[] playerWeapons;
    [SerializeField]
    private float weaponSwitchTime;
    private int currentWeapon;

    [SerializeField]
    private PlayerStats playerStats;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private PlayerState currentState;

    public PlayerState GetState => currentState;
    public PlayerStats GetStats => playerStats;
    public float CurrentHealthRatio => (float)currentHealth / playerStats.maxHealth;
    public float GetWeaponSpeed => playerWeapons[currentWeapon].GetStats.attackSpeed;
    public float GetWeaponSwitchTime => weaponSwitchTime;

    private Coroutine switchCoroutine;

    private void Awake()
    {
        InitStats();
    }

    public void InitStats()
    {
        if (!playerStats)
        {
            Debug.Log("NULL PLAYER STATS!");
            return;
        }

        currentHealth = playerStats.maxHealth;
    }

    public void MovePlayer(Transform newLocation)
    {
        transform.parent = newLocation;
        transform.localPosition = Vector3.zero;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= Math.Max(damage - playerStats.armorValue, 0);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            currentState = PlayerState.Dead;
            IsDead?.Invoke();
        }
    }

    public void Prepare()
    {
        currentState = PlayerState.Preparing;
    }

    public void Attack()
    {
        currentState = PlayerState.Attacking;
    }

    public IEnumerator SwitchWeapon()
    {
        while(currentState != PlayerState.Preparing)
        {
            if(currentState == PlayerState.Dead)
                yield break;

            yield return null;
        }

        currentState = PlayerState.Switching;
        IsSwitching?.Invoke();
        yield return new WaitForSeconds(weaponSwitchTime);
        playerWeapons[currentWeapon].gameObject.SetActive(false);
        currentWeapon++;
        if (currentWeapon > playerWeapons.Length - 1)
        {
            currentWeapon = 0;
        }
        playerWeapons[currentWeapon].gameObject.SetActive(true);
        currentState = PlayerState.Preparing;

        switchCoroutine = null;
    }

    public void HandleWeaponSwitch()
    {
        if(switchCoroutine == null)
        {
            switchCoroutine = StartCoroutine(SwitchWeapon());
        }
    }
}
