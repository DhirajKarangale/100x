using TMPro;
using UnityEngine;
using System.Collections;

public class BetManager : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] Animator animBetMsg;
    [SerializeField] GameManager gameManager;
    [SerializeField] protected TMP_Text txtTimer;

    protected float timer;
    protected float timerStVal;
    protected Chip chipSelected;

    private void Start()
    {
        timerStVal = WebsocketManager.instance.gamesInfo.thread_count * WebsocketManager.instance.gamesInfo.thread_time;
    }


    private IEnumerator IEStartTimer()
    {
        while (timer > 0)
        {
            timer--;
            txtTimer.text = $"{timer}s";
            yield return new WaitForSecondsRealtime(1);
        }

        StopTimer();
    }


    private void ResetAllChips()
    {
        chipSelected = null;
        foreach (Chip chip in gameManager.chipSpawner.chips) chip.Reset();
    }


    internal virtual void StopTimer()
    {
        animBetMsg.GetComponentInChildren<TMP_Text>().text = "Stop Betting";
        animBetMsg.Play("Play");
        StopAllCoroutines();
        gameManager.isBetAllowed = false;
        timer = 0;
        txtTimer.text = "VS";
        ResetAllChips();
    }

    internal virtual void StartTimer(float stVal = -1)
    {
        animBetMsg.GetComponentInChildren<TMP_Text>().text = "Place your bets";
        animBetMsg.Play("Play");
        gameManager.isBetAllowed = true;
        if(stVal == -1) timer = timerStVal;
        else timer = stVal;
        StartCoroutine(IEStartTimer());
    }

    internal virtual void ResetData()
    {
        gameManager.isBetAllowed = false;
        timer = timerStVal;
        txtTimer.text = timer.ToString() + "s";
        ResetAllChips();
    }


    public void ButtonChip(Chip chip)
    {
        ResetAllChips();

        if (!gameManager.isGameStarted) return;
        if (!gameManager.isBetAllowed) return;

        chipSelected = chip;
        if (chipSelected.betAmount == -1) chipSelected.betAmount = gameManager.userData.coins;
    }
}