using TMPro;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Msgs")]
    [SerializeField] TMP_Text txtWait;
    [SerializeField] GameObject objMsg;
    [SerializeField] GameObject objWait;
    
    [Header("BaseRefrences")]
    [SerializeField] internal Animator animatorSt;
    [SerializeField] internal ChipSpawner chipSpawner;
    [SerializeField] internal BetManager betManager;
    [SerializeField] internal GameWebsocket websocket;
    [SerializeField] internal GameUserData userData;

    internal static Action OnFinishWinAnim;
    internal bool isGameStarted, isBetAllowed, isWaiting;


    private void Start()
    {
        OnFinishWinAnim += WinAnimFinish;
    }

    private void OnDestroy()
    {
        OnFinishWinAnim -= WinAnimFinish;
    }

    private IEnumerator IEWait(float time)
    {
        while (time > 0)
        {
            objWait.SetActive(true);
            txtWait.text = $"Wait for {time}s";
            time--;
            yield return new WaitForSecondsRealtime(1);
        }

        isWaiting = false;
    }


    protected virtual void WinAnimFinish()
    {

    }

    internal virtual void NewGame(float stTimer = -1)
    {
        isWaiting = false;
        animatorSt.Play("Play");
        objWait.SetActive(false);
        SoundManager.instance?.Vibration(1);
        StopAllCoroutines();
    }

    internal virtual void UpdateData<T>(T data)
    {

    }

    internal void UpdateTime(float gameId, float extraTime = -1)
    {
        if (isWaiting) return;

        GamesInfoData data = WebsocketManager.instance.gamesInfo;
        if((data.thread_count - gameId) > 2)
        {
            NewGame((data.thread_count - gameId) * data.thread_time);
            return;
        }
        
        float time = (data.thread_count - gameId) * data.thread_time + data.game_wait;
        if (extraTime != -1) time = extraTime + data.game_wait + 1;
        
        StopAllCoroutines();
        StartCoroutine(IEWait(time));
    }

    internal void DisplayMsg(string msg)
    {
        objMsg.GetComponentInChildren<TMP_Text>().text = msg;
        objMsg.SetActive(true);
    }


    public void ButtonBack()
    {
        WebsocketManager.instance.Close();
        string msg = "Leaving Game...";
        Loading.instance.LoadLevel(1, 3, msg, 0, null);
    }

    public void ButtonURL(string url)
    {
        Application.OpenURL(url);
    }

    public void ButtonShop()
    {
        Application.OpenURL(PlayerPrefs.GetString("Deposit", "https://100x-ui-v2-0.pages.dev/"));
    }
}