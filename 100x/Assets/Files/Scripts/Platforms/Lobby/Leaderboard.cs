using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] LeaderboardItem prefabItem;


    private void Start()
    {
        StartCoroutine(IEGetData());
    }


    private IEnumerator IEGetData()
    {
        UnityWebRequest request = UnityWebRequest.Get(APIs.leaderboard);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("AccessToken", ""));

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            DataLeaderboard[] datas = JsonHelper.FromJson<DataLeaderboard>(jsonData);
            Reset();
            SetLeaderboard(datas);
        }
        else
        {
            yield return new WaitForSecondsRealtime(5);
            StartCoroutine(IEGetData());
        }
    }


    private void Reset()
    {
        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }
    }

    private void SetLeaderboard(DataLeaderboard[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            LeaderboardItem item = Instantiate(prefabItem);
            item.transform.SetParent(content);
            item.transform.localScale = Vector3.one;
            item.SetData(i + 1, data[i]);
        }
    }
}

[Serializable]
public class DataLeaderboard
{
    public string email;
    public double coins;
    public int total_winnings;
    public string picture;
    public DateTime created_at;
}