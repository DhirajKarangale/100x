using TMPro;
using UnityEngine;

public class UpBetManager : BetManager
{
    [SerializeField] UpManager manager;
    [SerializeField] TMP_Text txtBet2;
    [SerializeField] TMP_Text txtBet7;
    [SerializeField] TMP_Text txtBet8;

    private int bet2;
    private int bet7;
    private int bet8;


    private void UpdateData()
    {
        txtBet2.text = bet2.ToString() + "/";
        txtBet7.text = bet7.ToString() + "/";
        txtBet8.text = bet8.ToString() + "/";
    }

    internal override void ResetData()
    {
        base.ResetData();

        bet2 = 0;
        bet7 = 0;
        bet8 = 0;
        UpdateData();
    }

    public void ButtonBetLeft()
    {
        if (chipSelected == null) return;

        if (manager.userData.coins < chipSelected.betAmount)
        {
            manager.DisplayMsg("Not have enough coins");
            return;
        }

        manager.websocket.GetComponent<UpWebsocket>().SentData(chipSelected.betAmount, 0, 0);
        manager.chipSpawner.SpawnChip(chipSelected, chipSelected.transform.position, manager.chipSpawner.containers[1].position);
        bet2 += chipSelected.betAmount;
        UpdateData();
    }

    public void ButtonBetCenter()
    {
        if (chipSelected == null) return;

        if (manager.userData.coins < chipSelected.betAmount)
        {
            manager.DisplayMsg("Not have enough coins");
            return;
        }

        manager.websocket.GetComponent<UpWebsocket>().SentData(0, chipSelected.betAmount, 0);
        manager.chipSpawner.SpawnChip(chipSelected, chipSelected.transform.position, manager.chipSpawner.containers[2].position);
        bet7 += chipSelected.betAmount;
        UpdateData();
    }

    public void ButtonBetRight()
    {
        if (chipSelected == null) return;

        if (manager.userData.coins < chipSelected.betAmount)
        {
            manager.DisplayMsg("Not have enough coins");
            return;
        }

        manager.websocket.GetComponent<UpWebsocket>().SentData(0, 0, chipSelected.betAmount);
        manager.chipSpawner.SpawnChip(chipSelected, chipSelected.transform.position, manager.chipSpawner.containers[3].position);
        bet8 += chipSelected.betAmount;
        UpdateData();
    }
}