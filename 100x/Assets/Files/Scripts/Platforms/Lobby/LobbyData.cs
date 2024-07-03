using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyData : MonoBehaviour
{
    [SerializeField] GameObject objButtonWithdrawl, objButtonAddCash;

    public static Action<DataUser> OnDataLoaded;
    internal DataGameInfo dataGameInfo;
    internal DataUser dataUser;
    private DataURL dataURL;
    private string accessToken;


    private void Start()
    {
        LoadData();
    }


    private IEnumerator IEGetUserData()
    {
        UnityWebRequest request = UnityWebRequest.Get(APIs.user);
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            dataUser = JsonUtility.FromJson<DataUser>(jsonData);
            OnDataLoaded?.Invoke(dataUser);
        }
        else
        {
            dataUser = new DataUser();
            dataUser.name = "User Name";
            dataUser.coins = 0;
            dataUser.total_winnings = 0;
            dataUser.total_money_added = 0;
            dataUser.email = "User Email";
            dataUser.pan_number = "00000000";
            dataUser.contact_number = "0000000000";
            dataUser.picture = "";
            dataUser.created_at = DateTime.Now.ToString();
            dataUser.updated_at = DateTime.Now.ToString();
            dataUser.access_token = "";

            OnDataLoaded?.Invoke(dataUser);
        }
    }

    private IEnumerator IEGetGameInfo()
    {
        UnityWebRequest request = UnityWebRequest.Get(APIs.gameInfo);
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            dataGameInfo = JsonUtility.FromJson<DataGameInfo>(jsonData);
            PlayerPrefs.SetString("WebSocketUrl", dataGameInfo.websocket_url);
        }
        else
        {
            Debug.Log("Game Info Error : " + request.error);
        }
    }

    private IEnumerator IEGetURL()
    {
        UnityWebRequest request = UnityWebRequest.Get(APIs.getUrl);
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            dataURL = JsonUtility.FromJson<DataURL>(jsonData);
            objButtonAddCash.SetActive(!string.IsNullOrEmpty(dataURL.input_url));
            objButtonWithdrawl.SetActive(!string.IsNullOrEmpty(dataURL.output_url));
            PlayerPrefs.SetString("Deposit", dataURL.input_url);
        }
        else
        {
            Debug.Log("Game Info Error : " + request.error);
        }
    }


    internal void LoadData()
    {
        accessToken = PlayerPrefs.GetString("AccessToken", "");

        StartCoroutine(IEGetURL());
        StartCoroutine(IEGetUserData());
        StartCoroutine(IEGetGameInfo());
    }






    public void ButtonAddCash(int btn)
    {
        string url = "https://100xgame.com/";
        if (btn == 1) url = dataURL.input_url;
        else if (btn == 2) url = dataURL.output_url;
        Application.OpenURL(url);
    }
}

[Serializable]
public class DataURL
{
    public string input_url;
    public string output_url;
    public string websocket_url;
}