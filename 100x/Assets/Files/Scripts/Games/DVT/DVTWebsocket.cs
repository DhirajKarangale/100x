using System;
using UnityEngine;
using UnityEngine.UI;

public class DVTWebsocket : GameWebsocket
{
    [Header("History")]
    [SerializeField] Color[] colorsHistory;
    [SerializeField] GameObject[] objHistory;

    
    protected override void NewGame()
    {
        // DataDVTGame data = JsonUtility.FromJson<DataDVTGame>(message);
        DataNewGame data = JsonUtility.FromJson<DataNewGame>(message);
        gameId = data.game_id;

        if (string.IsNullOrEmpty(preThread) || !preThread.Equals(data.created_at)) manager.NewGame();
    }

    protected override void UpdateGame()
    {
        DataDVTGame data = JsonUtility.FromJson<DataDVTGame>(message);

        if (string.IsNullOrEmpty(preThread) || !preThread.Equals(data.created_at))
        {
            preThread = data.created_at;
            manager.GetComponent<DVTManager>().UpdateData(data);
        }
    }

    protected override void SetHistory(string jsonData)
    {
        DataDVTHistory[] datas = JsonHelper.FromJson<DataDVTHistory>(jsonData);

        for (int i = 0; i < 10; i++)
        {
            int idx = datas.Length - 1 - i;
            if (idx < 0) idx = 0;

            if (datas == null || datas[idx] == null || datas[idx].winner == null) continue;

            DataDVTData dataWinner = datas[idx].winner;

            Color color = colorsHistory[0];
            string winnerTxt = "D";

            if (dataWinner.tie == 1)
            {
                color = colorsHistory[1];
                winnerTxt = "Tie";
            }
            else if (dataWinner.tiger == 1)
            {
                color = colorsHistory[2];
                winnerTxt = "T";
            }

            objHistory[i].GetComponent<Image>().color = color;
            TMPro.TMP_Text txt = objHistory[i].GetComponentInChildren<TMPro.TMP_Text>();
            txt.fontSize = 60;
            if (winnerTxt == "Tie") txt.fontSize = 45;
            txt.text = winnerTxt;
        }
    }


    internal void SentData(int dragon, int tie, int tiger)
    {
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(gameName)) return;

        string dragonBet = dragon.ToString();
        string tieBet = tie.ToString();
        string tigerBet = tiger.ToString();
        string totalBet = (dragon + tie + tiger).ToString();

        string body = $"{{\"game_id\":\"{gameId}\",\"game_type\":\"{gameName}\",\"game_amount\":{{\"dragon\":{dragonBet},\"tie\":{tieBet},\"tiger\":{tigerBet}}},\"bid_amount_total\":{totalBet}}}";
        SentBidData(body);
    }
}


[System.Serializable]
public class DataDVTGame
{
    public int user_count;
    public string user_name;
    public DataDVTData total_amount;
    public string game_id;
    public float thread_id;
    public DataDVTData winner_data;
    public string created_at;
}

[System.Serializable]
public class DataDVTData
{
    public int dragon;
    public int tie;
    public int tiger;
}

[System.Serializable]
public class DataDVTHistory
{
    public string game_id;
    public string game_type;
    public DataDVTData amount;
    public int players;
    public DataDVTData winner;
    public DateTime created_at;
    public DateTime updated_at;
}