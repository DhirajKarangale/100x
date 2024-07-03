using TMPro;
using UnityEngine;

public class UpManager : GameManager
{
    [Header("Refrences")]
    [SerializeField] DiceManager diceManager;
    [SerializeField] Users users;

    [Header("Bet")]
    [SerializeField] TMP_Text txtBetTwo;
    [SerializeField] TMP_Text txtBetSeven;
    [SerializeField] TMP_Text txtBetEight;
    [SerializeField] TMP_Text txtUser;


    private void ResetData()
    {
        txtBetTwo.text = "0";
        txtBetSeven.text = "0";
        txtBetEight.text = "0";

        chipSpawner.ResetData();
        betManager.ResetData();
        diceManager.ResetData();
    }

    private void GetResult(int tow, int seven, int eight)
    {
        betManager.StopTimer();
        diceManager.Begin(tow, seven, eight);
    }

    private void UpdateBet(Data7UpGame data)
    {
        users.UpdateNames(data.user_name);
        txtUser.text = data.user_count.ToString();
        txtBetTwo.text = data.total_amount.two_to_six.ToString();
        txtBetSeven.text = data.total_amount.seven.ToString();
        txtBetEight.text = data.total_amount.eight_to_tweleve.ToString();
        chipSpawner.SpawnChip(3);
    }


    protected override void WinAnimFinish()
    {
        websocket.GetComponent<UpWebsocket>().UpdateHistory();
        isGameStarted = false;
        isBetAllowed = false;
        ResetData();
        userData.GetUserData();
    }

    internal override void NewGame(float stTimer = -1)
    {
        base.NewGame();
        users.UpdateImages();
        isGameStarted = true;
        ResetData();
        betManager.StartTimer(stTimer);
    }

    internal override void UpdateData<T>(T gameData)
    {
        if (gameData is Data7UpGame data)
        {
            if (!isGameStarted) UpdateTime(data.thread_id);

            if (data.winner_data.two_to_six != 0 || data.winner_data.seven != 0 || data.winner_data.eight_to_tweleve != 0)
            {
                txtUser.text = data.user_count.ToString();
                GetResult(data.winner_data.two_to_six, data.winner_data.seven, data.winner_data.eight_to_tweleve);
                return;
            }

            UpdateBet(data);
        }
    }
}