using System;
using UnityEngine;
using System.Collections;

public class AndarBaharTurn : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text txtUserTime;
    [SerializeField] Animator animatorPlayer;
    [SerializeField] GameManager gameManager;
    [SerializeField] AndarBaharBetManager betManager;
    [SerializeField] AndarBaharUser[] andarBaharUsers;
    [SerializeField] TMPro.TMP_Text[] txtRewards;
    private int currUser = -1;


    private void Awake()
    {
        ResetData();
    }


    private IEnumerator IEBet(String total_amount)
    {
        yield return new WaitForSecondsRealtime(3);

        AndarBaharWinnerData[] totalAmountData = JsonHelper.FromJson<AndarBaharWinnerData>(total_amount);

        if (currUser < totalAmountData.Length && totalAmountData[currUser % totalAmountData.Length] != null)
            andarBaharUsers[currUser % andarBaharUsers.Length].UpdateData(totalAmountData[currUser % totalAmountData.Length].ander, totalAmountData[currUser % totalAmountData.Length].bahar);

        yield return new WaitForSecondsRealtime(0.8f);
        andarBaharUsers[currUser % andarBaharUsers.Length].PlayAnim("Idle");
    }

    private IEnumerator IEStartUserTimer()
    {
        float timer = 4;
        txtUserTime.text = timer + "s";

        while (timer > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            timer--;
            txtUserTime.text = timer + "s";
        }
    }


    private void DisableAllAnims()
    {
        animatorPlayer.Play("Idle");
        foreach (AndarBaharUser user in andarBaharUsers) user.PlayAnim("Idle");
    }

    private void UpdateReward(float amount)
    {
        foreach (var txt in txtRewards) txt.text = amount.ToString("F2") + "x";
    }


    internal void UpdateTurn(int threadId, String total_amount)
    {
        if (threadId > 10 || !gameManager.isGameStarted) return;
        if (threadId <= 5) UpdateReward(2.25f);
        else if (threadId < 10) UpdateReward(2f);

        currUser = threadId - 1;
        DisableAllAnims();
        StopAllCoroutines();
        StartCoroutine(IEStartUserTimer());

        if ((currUser % andarBaharUsers.Length == 2))
        {
            betManager.buttonAndar.interactable = true;
            betManager.buttonBahar.interactable = true;
        }
        else
        {
            betManager.buttonAndar.interactable = false;
            betManager.buttonBahar.interactable = false;
            StartCoroutine(IEBet(total_amount));
        }

        andarBaharUsers[currUser % andarBaharUsers.Length].PlayAnim("Play");
    }

    internal void DisablePlayer()
    {
        DisableAllAnims();
    }

    internal void ResetData()
    {
        currUser = -1;
        DisableAllAnims();
        foreach (AndarBaharUser user in andarBaharUsers) user.SetName("User");
        betManager.buttonAndar.interactable = false;
        betManager.buttonBahar.interactable = false;
    }
}