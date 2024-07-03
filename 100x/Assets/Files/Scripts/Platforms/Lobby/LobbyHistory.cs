using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyHistory : MonoBehaviour
{
    [SerializeField] HistoryItem prefabHistoryItem;
    [SerializeField] PageManager pageManager;
    [SerializeField] Transform[] pages;

    private void Awake()
    {
        pageManager.OnPageClicked += OnPageClicked;
    }

    private void OnDestroy()
    {
        pageManager.OnPageClicked -= OnPageClicked;
    }


    private IEnumerator IEGetHistory(int page)
    {
        string url = APIs.history + page;

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("AccessToken", ""));

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            DataHistory dataHistory = JsonUtility.FromJson<DataHistory>(jsonData);
            UpdateHistory(page, dataHistory);
        }
        else
        {
            Debug.Log("History : " + request.error);
        }
    }

    private void UpdateHistory(int page, DataHistory dataHistory)
    {
        for (int i = 0; i < dataHistory.data.Length; i++)
        {
            if (dataHistory.data[i] == null) return;

            HistoryItem item = Instantiate(prefabHistoryItem);
            item.transform.SetParent(pages[page - 1]);
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.SetData(dataHistory.data[i]);
        }
    }


    private void OnPageClicked(int idx)
    {
        if (pages[idx - 1].childCount > 0) return;
        StartCoroutine(IEGetHistory(idx));
    }
}

[Serializable]
public class DataHistory
{
    public DataHistoryItem[] data;
    public int total_records;
}

[Serializable]
public class DataHistoryItem
{
    public int bid_amount;
    public string bid_on;
    public string created_at;
    public string game_type;
    public string game_winner;
}