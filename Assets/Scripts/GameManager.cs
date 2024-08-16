using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private RestUI restUI;

    [SerializeField]
    private PlayerScript player;
    [SerializeField]
    private CombatManager combatManager;

    [SerializeField]
    private Transform playerRestSpawn;

    private void Start()
    {
        PlayerScript.IsDead += StartRest;
        StartRest();
    }

    public void StartCombat()
    {
        restUI.Show(false);
        combatManager.InitCombat();
    }

    public void StartRest()
    {
        restUI.Show(true);
        player.MovePlayer(playerRestSpawn);
        restUI.UpdateHealth(player.CurrentHealthRatio);
    }

    public void HealPlayer()
    {
        player.InitStats();
        restUI.UpdateHealth(player.CurrentHealthRatio);
    }
}
