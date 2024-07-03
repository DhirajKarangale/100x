using System;
using UnityEngine;
using System.Text;
using System.Collections;
using UnityEngine.Networking;

public class GameWebsocket : MonoBehaviour
{
    [SerializeField] protected GameManager manager;

    [SerializeField] protected string gameName;
    protected string channelId;
    protected string accessToken;
    protected string gameId;
    protected string message;
    protected string preThread;
    protected bool isThread;
    internal bool isBetted;


    private void Start()
    {
        channelId = WebsocketManager.instance.gamesInfo.channel_id;
        accessToken = PlayerPrefs.GetString("AccessToken");
        WebsocketManager.instance.OnDataReceived += OnDataReceived;
        UpdateHistory();
        StartCoroutine(IECheckData());
    }

    private void OnDestroy()
    {
        WebsocketManager.instance.OnDataReceived -= OnDataReceived;
    }


    private IEnumerator IEUpdateHistory()
    {
        string url = APIs.gameHistory + gameName;

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            SetHistory(jsonData);
        }
        else
        {
            Debug.Log("History Error: " + request.error);
        }
    }

    private IEnumerator IESentBidData(string body)
    {
        isBetted = false;

        UnityWebRequest request = new UnityWebRequest(APIs.gameBid, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            DataGameBet data = JsonUtility.FromJson<DataGameBet>(jsonData);
            manager.userData.UpdateCoins(data.remaining_coins);
            isBetted = true;
            CancelInvoke();
            Invoke(nameof(InResetBet), 5);

            SoundManager soundManager = SoundManager.instance;
            if (soundManager) soundManager.Play(soundManager.clipCoin);
        }
        else
        {
            isBetted = false;
            Debug.Log("Error: " + request.error);
            manager.DisplayMsg("Bet stopped");
        }
    }

    private IEnumerator IECheckData()
    {
        while (true)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (isThread) UpdateGame();
                else if (!manager.isGameStarted) NewGame();
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private void OnDataReceived(string channelId, string message, string rootId)
    {
        if (!this.channelId.Equals(channelId)) return;

        isThread = !string.IsNullOrEmpty(rootId);
        this.message = message;
    }

    private void InResetBet()
    {
        isBetted = false;
    }


    protected void SentBidData(string body)
    {
        StartCoroutine(IESentBidData(body));
    }


    protected virtual void NewGame()
    {

    }

    protected virtual void UpdateGame()
    {

    }

    protected virtual void SetHistory(string jsonData)
    {

    }


    internal void UpdateHistory()
    {
        StartCoroutine(IEUpdateHistory());
    }
}