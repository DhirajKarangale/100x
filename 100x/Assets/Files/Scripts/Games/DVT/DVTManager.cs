using TMPro;
using UnityEngine;

public class DVTManager : GameManager
{
    [Header("Refrences")]
    [SerializeField] Users users;
    [SerializeField] CardsManager cardsManager;

    [Header("Anims")]
    [SerializeField] Animator[] animBoard;
    [SerializeField] Animator[] animWins;

    [Header("Bet")]
    [SerializeField] TMP_Text txtBetDragon;
    [SerializeField] TMP_Text txtBetTie;
    [SerializeField] TMP_Text txtBetTiger;
    [SerializeField] TMP_Text txtUsers;
    private int winner;


    private void ResetData()
    {
        animBoard[0].Play("Idle");
        animBoard[1].Play("Idle");

        txtBetDragon.text = "0";
        txtBetTie.text = "0";
        txtBetTiger.text = "0";

        chipSpawner.ResetData();
        betManager.ResetData();
        cardsManager.ResetData();
    }

    private void GetResult(int win)
    {
        winner = win;
        betManager.StopTimer();
        cardsManager.DVTStart(win);
    }

    private void UpdateBet(DataDVTGame data)
    {
        users.UpdateNames(data.user_name);
        txtBetDragon.text = data.total_amount.dragon.ToString();
        txtBetTie.text = data.total_amount.tie.ToString();
        txtBetTiger.text = data.total_amount.tiger.ToString();
        txtUsers.text = data.user_count.ToString();
        chipSpawner.SpawnChip(3);
    }


    protected override void WinAnimFinish()
    {
        ResetData();
        userData.GetUserData();
        websocket.UpdateHistory();
        isGameStarted = false;
        isBetAllowed = false;

        SoundManager soundManager = SoundManager.instance;
        if (soundManager) soundManager.Play(soundManager.clipDragon);
        animWins[winner].Play("Play");

        if (winner != 0) animBoard[winner - 1].Play("Play");
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
        if (gameData is DataDVTGame data)
        {
            if (!isGameStarted) UpdateTime(data.thread_id);

            if (data.winner_data.dragon != 0 || data.winner_data.tie != 0 || data.winner_data.tiger != 0)
            {
                txtUsers.text = data.user_count.ToString();
                if (data.winner_data.tie == 1) GetResult(0);
                else if (data.winner_data.dragon == 1) GetResult(1);
                else if (data.winner_data.tiger == 1) GetResult(2);
                return;
            }

            UpdateBet(data);
        }
    }
}