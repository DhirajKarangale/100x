using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class CrWebsocket : GameWebsocket
{
    [SerializeField] Image[] objHistory;


    private IEnumerator IECashOut(float height)
    {
        string body = $"{{\"height\":{height},\"game_id\":\"{gameId}\"}}";

        UnityWebRequest request = new UnityWebRequest(APIs.crashCashOut, "POST");
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
            manager.GetComponent<CrManager>().CashOutSucess();
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }


    protected override void NewGame()
    {
        // DataCrGame data = JsonUtility.FromJson<DataCrGame>(message);
        DataNewGame data = JsonUtility.FromJson<DataNewGame>(message);
        gameId = data.game_id;

        if (string.IsNullOrEmpty(preThread) || !preThread.Equals(data.created_at)) manager.NewGame();
    }

    protected override void UpdateGame()
    {
        DataCrGame data = JsonUtility.FromJson<DataCrGame>(message);

        if (string.IsNullOrEmpty(preThread) || !preThread.Equals(data.created_at))
        {
            preThread = data.created_at;
            manager.GetComponent<CrManager>().UpdateData(data);
        }
    }

    protected override void SetHistory(string jsonData)
    {
        DataCrHistory[] datas = JsonHelper.FromJson<DataCrHistory>(jsonData);

        for (int i = 0; i < 10; i++)
        {
            int idx = datas.Length - 1 - i;
            if (idx < 0) idx = 0;

            string winnerData = datas[idx].winner;
            float winner = 0;
            if (!string.IsNullOrEmpty(winnerData)) winner = float.Parse(winnerData);

            Color color = Color.blue;
            if (winner > 5) color = Color.red;
            else if (winner > 10) color = Color.magenta;

            objHistory[i].color = color;
            objHistory[i].GetComponentInChildren<TMPro.TMP_Text>().text = winner.ToString("F2") + "x";
        }
    }


    internal void SentData(int betAmount)
    {
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(gameName)) return;

        string body = $"{{\"game_id\":\"{gameId}\",\"game_type\":\"{gameName}\",\"game_amount\":null,\"bid_amount_total\":{betAmount}}}";
        SentBidData(body);
    }

    internal void CashOut(float height)
    {
        StartCoroutine(IECashOut(height));
    }
}

[System.Serializable]
public class DataCrGame
{
    public int user_count;
    public int total_amount;
    public string game_id;
    public float thread_id;
    public float winner_data;
    public string created_at;
}

[System.Serializable]
public class DataCrHistory
{
    public string game_id;
    public string game_type;
    public string amount;
    public int players;
    public string winner;
    public string created_at;
    public string updated_at;
}