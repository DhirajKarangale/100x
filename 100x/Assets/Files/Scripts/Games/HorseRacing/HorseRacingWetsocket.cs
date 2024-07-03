using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class HorseRacingWetsocket : GameWebsocket
{
    [Header("History")]
    [SerializeField] Color[] colorsHistory;
    [SerializeField] GameObject[] objHistory;


    protected override void NewGame()
    {
        DataNewGame data = JsonUtility.FromJson<DataNewGame>(message);
        gameId = data.game_id;

        if (string.IsNullOrEmpty(preThread) || !preThread.Equals(data.created_at)) manager.NewGame();
    }

    protected override void UpdateGame()
    {
        DataHorseRacing data = JsonUtility.FromJson<DataHorseRacing>(message);

        if (string.IsNullOrEmpty(preThread) || !preThread.Equals(data.created_at))
        {
            preThread = data.created_at;
            manager.GetComponent<HorseRacingManager>().UpdateData(data);
        }
    }

    protected override void SetHistory(string jsonData)
    {
        DataHorseRacingHistory[] datas = JsonHelper.FromJson<DataHorseRacingHistory>(jsonData);

        for (int i = 0; i < 10; i++)
        {
            int idx = datas.Length - 1 - i;
            if (idx < 0) idx = 0;

            if (datas == null || datas[idx] == null || datas[idx].winner == null) continue;

            DataHorseRacingHourses dataWinner = datas[idx].winner;

            Color color = colorsHistory[0];
            string winnerTxt = "1";

            if(dataWinner.horse2 == 1)
            {
                winnerTxt = "2";
                color = colorsHistory[0];
            }
            else if(dataWinner.horse3 == 1)
            {
                winnerTxt = "3";
                color = colorsHistory[1];
            }
            else if(dataWinner.horse4 == 1)
            {
                winnerTxt = "4";
                color = colorsHistory[1];
            }
            else if(dataWinner.horse5 == 1)
            {
                winnerTxt = "5";
                color = colorsHistory[2];
            }
            else if(dataWinner.horse6 == 1)
            {
                winnerTxt = "6";
                color = colorsHistory[2];
            }

            objHistory[i].GetComponent<Image>().color = color;
            objHistory[i].GetComponentInChildren<TMPro.TMP_Text>().text = winnerTxt;
        }
    }


    internal void SentData(int[] bids)
    {
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(gameName)) return;

        string totalBet = bids.Sum().ToString();
        string body = $"{{\"game_id\":\"{gameId}\",\"game_type\":\"{gameName}\",\"game_amount\":{{\"horse1\":{bids[0]},\"horse2\":{bids[1]},\"horse3\":{bids[2]},\"horse4\":{bids[3]},\"horse5\":{bids[4]},\"horse6\":{bids[5]}}},\"bid_amount_total\":{totalBet}}}";
        
        SentBidData(body);
    }
}




[Serializable]
public class DataHorseRacing
{
    public int user_count;
    public string user_name;
    public DataHorseRacingHourses total_amount;
    public string game_id;
    public int thread_id;
    public DataHorseRacingHourses winner_data;
    public string created_at;
}

[Serializable]
public class DataHorseRacingHistory
{
    public string game_id;
    public string game_type;
    public DataHorseRacingHourses amount;
    public string players;
    public DataHorseRacingHourses winner;
    public string created_at;
    public string updated_at;
}

[Serializable]
public class DataHorseRacingHourses
{
    public int horse1;
    public int horse2;
    public int horse3;
    public int horse4;
    public int horse5;
    public int horse6;
}