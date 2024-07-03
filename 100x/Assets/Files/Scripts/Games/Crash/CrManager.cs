using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CrManager : GameManager
{
    [Header("Refrences")]
    [SerializeField] CrUserCashout userCashout;
    [SerializeField] Image imgCashOut;
    [SerializeField] internal RocketController rocketController;

    [Header("Screen")]
    [SerializeField] GameObject objChips;
    [SerializeField] GameObject objBetScreen;
    [SerializeField] GameObject objRocketScreen;

    [Header("Bet")]
    [SerializeField] TMP_Text txtUser;
    [SerializeField] TMP_Text txtBetAmount;

    internal bool isCashOut, isRocket;
    private bool isGameOver;

    private IEnumerator IEStartRocket(float height, float threadId)
    {
        yield return new WaitForSecondsRealtime(1);

        chipSpawner.ResetData();
        objBetScreen.SetActive(false);
        objChips.SetActive(false);
        objRocketScreen.SetActive(true);

        yield return new WaitForSecondsRealtime(1);

        rocketController.Begin(height, threadId);
        isRocket = true;
    }

    private IEnumerator IEStopRocket()
    {
        yield return new WaitForSecondsRealtime(3);

        // ResetData();
        objChips.SetActive(true);
        objBetScreen.SetActive(true);
        objRocketScreen.SetActive(false);
        isGameStarted = false;
        betManager.GetComponent<CrBetManager>().isBetted = false;
    }


    private void ResetData()
    {
        isRocket = false;
        isCashOut = false;
        isGameOver = false;
        txtUser.text = "0";
        txtBetAmount.text = "0";

        objChips.SetActive(true);
        objBetScreen.SetActive(true);
        objRocketScreen.SetActive(false);

        chipSpawner.ResetData();
        betManager.ResetData();
        rocketController.Reset();
        imgCashOut.color = Color.green;
    }

    private void UpdateBet(DataCrGame data)
    {
        chipSpawner.SpawnChip(1);
        txtUser.text = data.user_count.ToString();
        txtBetAmount.text = data.total_amount.ToString();
    }

    private void GetResult(float height, float threadId)
    {
        betManager.StopTimer();

        if (!betManager.GetComponent<CrBetManager>().isBetted) imgCashOut.color = Color.gray;
        chipSpawner.ResetData();
        float score = Mathf.Min(21, height);

        if (((height * 5) - (2 * Math.Abs(7 - threadId)) >= 4f || height < 1f)) StartCoroutine(IEStartRocket(score, threadId));
    }

    protected override void WinAnimFinish()
    {
        websocket.UpdateHistory();
        StartCoroutine(IEStopRocket());
    }

    internal override void NewGame(float stTimer = -1)
    {
        base.NewGame();
        ResetData();
        isGameStarted = true;
        betManager.StartTimer(stTimer);
    }

    internal override void UpdateData<T>(T gameData)
    {
        if (gameData is DataCrGame data)
        {
            if (!isGameStarted)
            {
                GamesInfoData gameInfo = WebsocketManager.instance.gamesInfo;
                if (gameInfo.thread_count - data.thread_id > 3)
                {
                    NewGame((gameInfo.thread_count - data.thread_id) * gameInfo.thread_time - 1);
                    return;
                }
            }

            if (!isGameStarted && !isGameOver) UpdateTime(data.thread_id, ((int)data.winner_data * 5) - ((data.thread_id - 7) * WebsocketManager.instance.gamesInfo.thread_time));

            if (!isGameOver && !isRocket && data.winner_data != 0)
            {
                isGameOver = true;
                isWaiting = true;
                txtUser.text = data.user_count.ToString();
                GetResult(data.winner_data, data.thread_id);
                return;
            }

            if (!isGameOver) UpdateBet(data);
        }
    }

    internal void CashOutSucess()
    {
        isCashOut = true;
        imgCashOut.color = Color.grey;
        userCashout.CashOut(PlayerPrefs.GetString("Name", "Name"));
    }


    public void ButtonCashOut()
    {
        if (isCashOut || !betManager.GetComponent<CrBetManager>().isBetted || !isRocket) return;
        websocket.GetComponent<CrWebsocket>().CashOut(rocketController.currScore);
    }
}