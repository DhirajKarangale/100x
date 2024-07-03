using UnityEngine;
using System.Collections.Generic;

public class AndarBaharWebSocket : GameWebsocket
{
    protected override void NewGame()
    {
        // DataAndarBahar data = JsonUtility.FromJson<DataAndarBahar>(message);
        DataNewGame data = JsonUtility.FromJson<DataNewGame>(message);
        gameId = data.game_id;

        if (string.IsNullOrEmpty(preThread) || !preThread.Equals(data.created_at)) manager.NewGame();
    }

    protected override void UpdateGame()
    {
        DataAndarBahar data = JsonUtility.FromJson<DataAndarBahar>(message);

        if (string.IsNullOrEmpty(preThread) || !preThread.Equals(data.created_at))
        {
            preThread = data.created_at;
            manager.GetComponent<AndarBaharManager>().UpdateData(data);
        }
    }


    internal void SentData(int andar, int bahar)
    {
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(gameName)) return;

        string andarBet = andar.ToString();
        string baharBet = bahar.ToString();
        string totalBet = (andar + bahar).ToString();

        string body = $"{{\"game_id\":\"{gameId}\",\"game_type\":\"{gameName}\",\"game_amount\":{{\"ander\":{andarBet},\"bahar\":{baharBet}}},\"bid_amount_total\":{totalBet}}}";
        SentBidData(body);
    }
}



[System.Serializable]
public class DataAndarBahar
{
    public int user_count;
    public string user_name;
    public string total_amount;
    public string game_id;
    public int thread_id;
    public AndarBaharWinnerData winner_data;
    public string created_at;
}


[System.Serializable]
public class AndarBaharWinnerData
{
    public int ander;
    public int bahar;
}