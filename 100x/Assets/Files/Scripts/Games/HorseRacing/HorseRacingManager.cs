using TMPro;
using UnityEngine;

public class HorseRacingManager : GameManager
{
    [Header("Refrences")]
    [SerializeField] GameObject objTimer;
    [SerializeField] Users users;
    [SerializeField] HorseManager horseManager;

    [Header("Bet")]
    [SerializeField] TMP_Text[] txtHorses;
    [SerializeField] TMP_Text txtUsers;
    private int winner;


    private void ResetData()
    {
        foreach (TMP_Text txt in txtHorses) txt.text = "0";

        chipSpawner.ResetData();
        betManager.ResetData();
        horseManager.ResetData();
    }

    private void GetResult(int win)
    {
        objTimer.SetActive(false);
        winner = win;
        betManager.StopTimer();
        horseManager.Begin(win);
    }

    private void UpdateBet(DataHorseRacing data)
    {
        users.UpdateNames(data.user_name);
        txtHorses[0].text = data.total_amount.horse1.ToString();
        txtHorses[1].text = data.total_amount.horse2.ToString();
        txtHorses[2].text = data.total_amount.horse3.ToString();
        txtHorses[3].text = data.total_amount.horse4.ToString();
        txtHorses[4].text = data.total_amount.horse5.ToString();
        txtHorses[5].text = data.total_amount.horse6.ToString();
        txtUsers.text = data.user_count.ToString();
        chipSpawner.SpawnChip(6, 0.05f);
    }


    protected override void WinAnimFinish()
    {
        // ResetData();
        userData.GetUserData();
        websocket.UpdateHistory();
        isGameStarted = false;
        isBetAllowed = false;
    }


    internal override void NewGame(float stTimer = -1)
    {
        base.NewGame();

        ResetData();
        objTimer.SetActive(true);
        users.UpdateImages();
        isGameStarted = true;
        betManager.StartTimer(stTimer);
    }

    internal override void UpdateData<T>(T gameData)
    {
        if (gameData is DataHorseRacing data)
        {
            if (!isGameStarted) UpdateTime(data.thread_id);

            if (data.winner_data.horse1 != 0 || data.winner_data.horse2 != 0 || data.winner_data.horse3 != 0 || data.winner_data.horse4 != 0 || data.winner_data.horse5 != 0 || data.winner_data.horse6 != 0)
            {
                isBetAllowed = false;
                txtUsers.text = data.user_count.ToString();
                if (data.winner_data.horse1 == 1) GetResult(0);
                else if (data.winner_data.horse2 == 1) GetResult(1);
                else if (data.winner_data.horse3 == 1) GetResult(2);
                else if (data.winner_data.horse4 == 1) GetResult(3);
                else if (data.winner_data.horse5 == 1) GetResult(4);
                else if (data.winner_data.horse6 == 1) GetResult(5);
                return;
            }

            if (isBetAllowed) UpdateBet(data);
        }
    }
}
