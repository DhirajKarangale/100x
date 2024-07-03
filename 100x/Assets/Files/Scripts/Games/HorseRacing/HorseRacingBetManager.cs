using UnityEngine;

public class HorseRacingBetManager : BetManager
{
    [SerializeField] HorseRacingManager manager;
    [SerializeField] TMPro.TMP_Text[] txtUserBets;
    private int[] userBets = new int[6];

    internal override void ResetData()
    {
        base.ResetData();

        for (int i = 0; i < 6; i++)
        {
            userBets[i] = 0;
            txtUserBets[i].text = "0/";
        }
    }

    public void ButtonBetHorse(int horse)
    {
        if (chipSelected == null) return;

        if (manager.userData.coins < chipSelected.betAmount)
        {
            manager.DisplayMsg("Not have enough coins");
            return;
        }

        int[] bids = new int[6];
        bids[horse - 1] = chipSelected.betAmount;
        userBets[horse - 1] += chipSelected.betAmount;
        txtUserBets[horse - 1].text = userBets[horse - 1].ToString() + "/";

        manager.websocket.GetComponent<HorseRacingWetsocket>().SentData(bids);
        manager.chipSpawner.SpawnChip(chipSelected, chipSelected.transform.position, manager.chipSpawner.containers[horse].position, 0.07f);
    }
}