using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AndarBaharManager : GameManager
{
    [Header("Refrences")]
    [SerializeField] internal AndarBaharTurn turn;
    [SerializeField] CardsManager cardsManager;
    [SerializeField] TMPro.TMP_Text txtUsers;
    [SerializeField] TMPro.TMP_Text txtRound;
    [SerializeField] Animator[] animWins;
    [SerializeField] AndarBaharUser[] users;
    private int winner;


    private void ResetData()
    {
        // chipSpawner.ResetData();
        txtRound.text = "Round 1";
        turn.ResetData();
        betManager.ResetData();
        foreach (Animator anims in animWins) anims.Play("Idle");
    }

    private void GetResult(int win)
    {
        txtRound.text = "Round 2";
        winner = win;
        betManager.StopTimer();
        cardsManager.AndarBaharStart(win);
    }

    private void UpdateBet(DataAndarBahar data)
    {
        if (data.thread_id == 1)
        {
            SetUserName(data.user_name);
            txtRound.text = "Round 1";
            if(!isGameStarted) NewGame();
        }

        if (data.thread_id == 6)
        {
            if (isGameStarted) cardsManager.AndarBaharMid();
            txtRound.text = "Round 2";
        }

        txtUsers.text = data.user_count.ToString();
        turn.UpdateTurn(data.thread_id, data.total_amount);
    }

    private void SetUserName(string userNames)
    {
        string[] names = userNames.Split(',').Distinct().ToArray(); ;

        for (int i = 0; i < 4; i++)
        {
            string currName = "User" + UnityEngine.Random.Range(100, 1000);
            if (i < names.Length && !string.IsNullOrEmpty(names[i])) currName = names[i];
            users[i % 4].SetName(currName);
        }
    }


    protected override void WinAnimFinish()
    {
        // websocket.UpdateHistory();
        isGameStarted = false;
        isBetAllowed = false;
        ResetData();
        userData.GetUserData();

        for (int i = 0; i < animWins.Length; i++)
        {
            if (i == winner) animWins[i].Play("Play");
            else animWins[i].Play("Lose");
        }
    }


    internal override void NewGame(float stTimer = -1)
    {
        // animatorSt.Play("Play");
        base.NewGame();

        ResetData();
        isGameStarted = true;
        userData.GetUserData();
        betManager.StartTimer(stTimer);
        cardsManager.AndarBaharNewGame();
    }

    internal override void UpdateData<T>(T gameData)
    {
        if (gameData is DataAndarBahar data)
        {
            if (!isGameStarted) UpdateTime(data.thread_id);

            if (data.winner_data.ander != 0 || data.winner_data.bahar != 0)
            {
                if (data.winner_data.ander == 1) GetResult(0);
                else if (data.winner_data.bahar == 1) GetResult(1);

                return;
            }

            UpdateBet(data);
        }
    }
}