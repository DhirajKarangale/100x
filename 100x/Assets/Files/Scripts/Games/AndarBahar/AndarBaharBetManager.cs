using System.Collections;
using UnityEngine;

public class AndarBaharBetManager : BetManager
{
    [SerializeField] AndarBaharManager manager;
    [SerializeField] TMPro.TMP_Text txtAndar, txtBahar;
    [SerializeField] internal UnityEngine.UI.Button buttonAndar, buttonBahar;
    private int amtAndar, amtBahar;


    internal override void ResetData()
    {
        base.ResetData();

        amtAndar = 0;
        amtBahar = 0;
        txtAndar.text = amtAndar.ToString();
        txtBahar.text = amtBahar.ToString();
    }


    public void ButtonBetAndar()
    {
        if (chipSelected == null) return;

        if (manager.userData.coins < chipSelected.betAmount)
        {
            manager.DisplayMsg("Not have enough coins");
            return;
        }

        manager.websocket.GetComponent<AndarBaharWebSocket>().SentData(chipSelected.betAmount, 0);
        // manager.chipSpawner.SpawnChip(chipSelected, chipSelected.transform.position, manager.chipSpawner.containers[1].position);

        StopAllCoroutines();
        StartCoroutine(IEUpdateAndarBet(chipSelected.betAmount));
    }

    public void ButtonBetBahar()
    {
        if (chipSelected == null) return;

        if (manager.userData.coins < chipSelected.betAmount)
        {
            manager.DisplayMsg("Not have enough coins");
            return;
        }

        manager.websocket.GetComponent<AndarBaharWebSocket>().SentData(0, chipSelected.betAmount);
        // manager.chipSpawner.SpawnChip(chipSelected, chipSelected.transform.position, manager.chipSpawner.containers[2].position);

        StopAllCoroutines();
        StartCoroutine(IEUpdateBaharBet(chipSelected.betAmount));
    }


    private IEnumerator IEUpdateAndarBet(int amount)
    {
        float timer = 20;

        while (timer > 0)
        {
            if (manager.websocket.isBetted)
            {
                amtAndar += amount;
                txtAndar.text = amtAndar.ToString();
                buttonAndar.interactable = false;
                buttonBahar.interactable = false;
                manager.turn.DisablePlayer();
                yield break;
            }

            yield return new WaitForSecondsRealtime(0.1f);
            timer -= 1;
        }
    }

    private IEnumerator IEUpdateBaharBet(int amount)
    {
        float timer = 20;

        while (timer > 0)
        {
            if (manager.websocket.isBetted)
            {
                amtBahar += amount;
                txtBahar.text = amtBahar.ToString();
                buttonAndar.interactable = false;
                buttonBahar.interactable = false;
                manager.turn.DisablePlayer();
                yield break;
            }

            yield return new WaitForSecondsRealtime(0.1f);
            timer -= 1;
        }
    }
}