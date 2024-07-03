using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrBetManager : BetManager
{
    [SerializeField] CrManager manager;
    [SerializeField] TMP_Text txtUserBet;
    [SerializeField] TMP_Text txtCashOut;
    [SerializeField] Slider sliderBetTime;

    internal bool isBetted;
    private int userBet;


    private IEnumerator IEStartTimer()
    {
        while (timer > 0)
        {
            sliderBetTime.value = (float)((float)timer / (float)timerStVal);
            yield return new WaitForSecondsRealtime(1);
        }
    }


    internal override void StopTimer()
    {
        base.StopTimer();
        StopAllCoroutines();
        sliderBetTime.value = 0;
        txtTimer.text = "Launching...";
    }

    internal override void StartTimer(float stVal = -1)
    {
        base.StartTimer();
        sliderBetTime.value = 1;
        StartCoroutine(IEStartTimer());
    }

    internal override void ResetData()
    {
        base.ResetData();
        userBet = 0;
        sliderBetTime.value = 1;
        txtUserBet.text = "0/";
    }

    internal void UpdateCash()
    {
        txtCashOut.text = (userBet * manager.rocketController.currScore).ToString("F4");
    }


    public void ButtonBet()
    {
        if (chipSelected == null) return;

        if (manager.userData.coins < chipSelected.betAmount)
        {
            manager.DisplayMsg("Not have enough coins");
            return;
        }

        isBetted = true;
        manager.websocket.GetComponent<CrWebsocket>().SentData(chipSelected.betAmount);
        manager.chipSpawner.SpawnChip(chipSelected, chipSelected.transform.position, manager.chipSpawner.containers[1].position);

        userBet += chipSelected.betAmount;
        txtUserBet.text = userBet.ToString() + "/";
    }
}