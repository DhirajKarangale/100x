using TMPro;
using UnityEngine;

public class DVTBetManager : BetManager
{
    [SerializeField] DVTManager manager;
    [SerializeField] TMP_Text txtBetDragon;
    [SerializeField] TMP_Text txtBetTie;
    [SerializeField] TMP_Text txtBetTiger;

    private int betDragon;
    private int betTie;
    private int betTiger;


    private void UpdateData()
    {
        txtBetDragon.text = betDragon.ToString() + "/";
        txtBetTie.text = betTie.ToString() + "/";
        txtBetTiger.text = betTiger.ToString() + "/";
    }


    internal override void ResetData()
    {
        base.ResetData();

        betDragon = 0;
        betTie = 0;
        betTiger = 0;
        UpdateData();
    }

    public void ButtonBetDragon()
    {
        if (chipSelected == null) return;

        if (manager.userData.coins < chipSelected.betAmount)
        {
            manager.DisplayMsg("Not have enough coins");
            return;
        }

        manager.websocket.GetComponent<DVTWebsocket>().SentData(chipSelected.betAmount, 0, 0);
        manager.chipSpawner.SpawnChip(chipSelected, chipSelected.transform.position, manager.chipSpawner.containers[1].position);
        betDragon += chipSelected.betAmount;
        UpdateData();
    }

    public void ButtonBetTie()
    {
        if (chipSelected == null) return;

        if (manager.userData.coins < chipSelected.betAmount)
        {
            manager.DisplayMsg("Not have enough coins");
            return;
        }

        manager.websocket.GetComponent<DVTWebsocket>().SentData(0, chipSelected.betAmount, 0);
        manager.chipSpawner.SpawnChip(chipSelected, chipSelected.transform.position, manager.chipSpawner.containers[2].position);
        betTie += chipSelected.betAmount;
        UpdateData();
    }

    public void ButtonBetTiger()
    {
        if (chipSelected == null) return;

        if (manager.userData.coins < chipSelected.betAmount)
        {
            manager.DisplayMsg("Not have enough coins");
            return;
        }

        manager.websocket.GetComponent<DVTWebsocket>().SentData(0, 0, chipSelected.betAmount);
        manager.chipSpawner.SpawnChip(chipSelected, chipSelected.transform.position, manager.chipSpawner.containers[3].position);
        betTiger += chipSelected.betAmount;
        UpdateData();
    }
}