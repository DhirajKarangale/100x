using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections;
using UnityEngine.Networking;

public class WebsocketManager : PersistentSingleton<WebsocketManager>
{
    private WebSocket socket;
    private string serverUrl;

    internal Action<string, string, string> OnDataReceived;
    internal GamesInfoData gamesInfo;


    private void OnDestroy()
    {
        Close();
    }


    private IEnumerator IEConnection()
    {
        if (!Lobby.instance || !Lobby.instance.isLoggedIn) yield break;

        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Close();
            Msg.instance.DisplayMsg("Your are not connected to internet", Color.white);
        }
        else
        {
            socket = new WebSocket(serverUrl);
            socket.OnOpen += OnWebSocketOpen;
            socket.OnMessage += OnMessageReceived;
            socket.Connect();
        }
    }


    private void OnWebSocketOpen(object sender, System.EventArgs e)
    {
        Authenticate();
    }

    private void OnMessageReceived(object sender, MessageEventArgs e)
    {
        if (e.Data.Contains("\"event\":\"posted\""))
        {
            DataWebSocket rootObject = JsonUtility.FromJson<DataWebSocket>(e.Data);
            PostWebSocket post = JsonUtility.FromJson<PostWebSocket>(rootObject.data.post);

            OnDataReceived?.Invoke(post.channel_id, post.message, post.root_id);
        }
    }


    private void Authenticate()
    {
        if (string.IsNullOrEmpty(gamesInfo.auth_token)) return;
        string authPayload = $"{{\"seq\": 1, \"action\": \"authentication_challenge\", \"data\": {{\"token\": \"{gamesInfo.auth_token}\"}}}}";
        socket.Send(authPayload);
    }


    internal void Connect(GamesInfoData gamesInfo)
    {
        Close();
        this.gamesInfo = gamesInfo;
        serverUrl = PlayerPrefs.GetString("WebSocketUrl", "wss://threads.saralgroups.com/api/v4/websocket");
        StartCoroutine(IEConnection());
    }

    internal void Close()
    {
        if (socket != null && socket.ReadyState == WebSocketState.Open)
        {
            gamesInfo = null;
            socket.Close();
        }
    }
}