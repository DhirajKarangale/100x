using TMPro;
using UnityEngine;

public class UpWebsocket : GameWebsocket
{
    [SerializeField] GameObject[] objHistory;

    protected override void NewGame()
    {
        // Data7UpGame data = JsonUtility.FromJson<Data7UpGame>(message);
        DataNewGame data = JsonUtility.FromJson<DataNewGame>(message);
        gameId = data.game_id;

        if (string.IsNullOrEmpty(preThread) || !preThread.Equals(data.created_at)) manager.NewGame();
    }

    protected override void UpdateGame()
    {
        Data7UpGame data = JsonUtility.FromJson<Data7UpGame>(message);

        if (string.IsNullOrEmpty(preThread) || !preThread.Equals(data.created_at))
        {
            preThread = data.created_at;
            manager.GetComponent<UpManager>().UpdateData(data);
        }
    }

    protected override void SetHistory(string jsonData)
    {
        Data7UpHistory[] datas = JsonHelper.FromJson<Data7UpHistory>(jsonData);

        for (int i = 0; i < 10; i++)
        {
            int idx = datas.Length - 1 - i;
            if (idx < 0) idx = 0;

            string history = "7";

            if (datas == null || datas[idx] == null || datas[idx].winner == null)
            {
                history = "7";
            }
            else
            {
                Data7UpData dataWinner = datas[idx].winner;
                if (dataWinner.two_to_six == 1) history = "2-6";
                else if (dataWinner.seven == 1) history = "7";
                else if (dataWinner.eight_to_tweleve == 1) history = "8-12";
            }

            objHistory[i].GetComponentInChildren<TMP_Text>().text = history;
        }
    }


    internal void SentData(int tow, int seven, int eight)
    {
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(gameName)) return;

        string towBet = tow.ToString();
        string sevenBet = seven.ToString();
        string eightBet = eight.ToString();
        string totalBet = (tow + seven + eight).ToString();

        string body = $"{{\"game_id\":\"{gameId}\",\"game_type\":\"{gameName}\",\"game_amount\":{{\"two_to_six\":{towBet},\"seven\":{sevenBet},\"eight_to_tweleve\":{eightBet}}},\"bid_amount_total\":{totalBet}}}";
        SentBidData(body);
    }
}


[System.Serializable]
public class Data7UpGame
{
    public int user_count;
    public string user_name;
    public Data7UpData total_amount;
    public string game_id;
    public float thread_id;
    public Data7UpData winner_data;
    public string created_at;
}

[System.Serializable]
public class Data7UpData
{
    public int two_to_six;
    public int eight_to_tweleve;
    public int seven;
}

[System.Serializable]
public class Data7UpHistory
{
    public string game_id;
    public string game_type;
    public Data7UpData amount;
    public int players;
    public Data7UpData winner;
    public string created_at;
    public string updated_at;
}