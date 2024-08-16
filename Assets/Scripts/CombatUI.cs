using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField]
    private GameObject combatArea;

    [SerializeField]
    private Slider hpSliderPlayer;
    [SerializeField]
    private Slider hpSliderEnemy;

    [SerializeField]
    private Image playerPrepareCounter;
    [SerializeField]
    private Image playerAttackCounter;
    [SerializeField]
    private Image playerSwitchCounter;

    [SerializeField]
    private Image enemyPrepareCounter;
    [SerializeField]
    private Image enemyAttackCounter;

    private Coroutine playerCoroutine;
    private Coroutine enemyCoroutine;

    public void Show(bool display)
    {
        StopPlayerCounters();
        StopEnemyCounters();

        combatArea.SetActive(display);
    }

    public void StopPlayerCounters()
    {
        if(playerCoroutine != null)
        {
            StopCoroutine(playerCoroutine);
            playerCoroutine = null;
        }

        playerAttackCounter.gameObject.SetActive(false);
        playerPrepareCounter.gameObject.SetActive(false);
        playerSwitchCounter.gameObject.SetActive(false);
    }

    public void StopEnemyCounters()
    {
        if(enemyCoroutine != null)
        {
            StopCoroutine(enemyCoroutine);
            enemyCoroutine = null;
        }

        enemyPrepareCounter.gameObject.SetActive(false);
        enemyAttackCounter.gameObject.SetActive(false);
    }

    public void UpdatePlayerHP(float value)
    {
        hpSliderPlayer.value = value;
    }

    public void UpdateEnemyHP(float value)
    {
        hpSliderEnemy.value = value;
    }

    private IEnumerator CounterCoroutine(Image counter, float countdown)
    {
        counter.gameObject.SetActive(true);
        float curCount = countdown;
        while (curCount > 0)
        {
            counter.fillAmount = curCount / countdown;
            curCount -= Time.deltaTime;
            yield return null;
        }
        counter.gameObject.SetActive(false);
    }

    public void ShowPlayerPrepareCounter(bool display)
    {
        playerPrepareCounter.gameObject.SetActive(display);
    }

    public void PlayerPrepareCounter(float value)
    {
        playerPrepareCounter.fillAmount = value;
    }

    public void StartPlayerAttackCounter(float countdown)
    {
        playerCoroutine = StartCoroutine(CounterCoroutine(playerAttackCounter, countdown));
    }

    public void StartPlayerSwitchCounter(float countdown)
    {
        StopPlayerCounters();
        playerCoroutine = StartCoroutine(CounterCoroutine(playerSwitchCounter, countdown));
    }

    public void StartEnemyPrepareCounter(float countdown)
    {
        enemyCoroutine = StartCoroutine(CounterCoroutine(enemyPrepareCounter, countdown));
    }

    public void StartEnemyAttackCounter(float countdown)
    {
        enemyCoroutine = StartCoroutine(CounterCoroutine(enemyAttackCounter, countdown));
    }
}
