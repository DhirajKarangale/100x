using System;
using UnityEngine;
using System.Text;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using System.IO;

public class Lobby : Singleton<Lobby>
{
    [SerializeField] Texture2D textureShare;
    [SerializeField] LobbyUser lobbyUser;
    [SerializeField] LobbyData lobbyData;
    [SerializeField] TMP_InputField inputService;
    [SerializeField] TMP_InputField inputRefral;
    [SerializeField] GameObject objMenuLogin;
    [SerializeField] GameObject objMenuLogout;

    [SerializeField] Game[] games;

    internal bool isLoggedIn;

    private void Start()
    {
        isLoggedIn = PlayerPrefs.GetInt("LoggedIn", 0) == 1;
        objMenuLogin.SetActive(!isLoggedIn);
        objMenuLogout.SetActive(isLoggedIn);
    }


    private IEnumerator IESupport(string body)
    {
        UnityWebRequest request = new UnityWebRequest(APIs.support, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("AccessToken"));
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Debug.Log("Data sent sucess");
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }

    private IEnumerator IESubmitRefral()
    {
        string url = $"{APIs.submitRefral}/{inputRefral.text}";
        string body = "";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("AccessToken"));
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            lobbyData.LoadData();
        }
        else
        {
            Msg.instance.DisplayMsg("Refral failed, try again", Color.white);
            Debug.Log("Error: " + request.error);
        }
    }


    public void ButtonLoginIn()
    {
        string msg = "Reaching Login Screen...";
        if (isLoggedIn) PlayerPrefs.SetInt("LogOut", 1);
        Loading.instance.LoadLevel(0, 2, msg, 0, null);
    }

    public void ButtonMail()
    {
        string emailBody = "";
        string emailSubject = "Ticket from - " + PlayerPrefs.GetString("Name", "Your Name");
        emailSubject = System.Uri.EscapeUriString(emailSubject);
        Application.OpenURL("mailto:contact@100x.com" + "?subject=" + emailSubject + "&body=" + emailBody);
    }

    public void ButtonUrl(string url)
    {
        Application.OpenURL(url);
    }

    public void ButtonGame(string game)
    {
        if (!isLoggedIn)
        {
            Msg.instance.DisplayMsg("You are not Logged In", Color.white);
            return;
        }

        if (lobbyData.dataGameInfo == null || lobbyData.dataGameInfo.games_info.Length == 0)
        {
            Msg.instance.DisplayMsg("Failed loading data try again", Color.white);
            lobbyData.LoadData();
            return;
        }

        GamesInfoData[] gamesInfo = lobbyData.dataGameInfo.games_info;
        int gameIdx = 0;
        int addressableIdx = 0;

        for (int i = 0; i < games.Length; i++)
        {
            if (game.Equals(games[i].name))
            {
                addressableIdx = games[i].index;
                break;
            }
        }

        for (int i = 0; i < gamesInfo.Length; i++)
        {
            if (game.Equals(gamesInfo[i].game_type))
            {
                gameIdx = i;
                break;
            }
        }

        WebsocketManager.instance.Connect(gamesInfo[gameIdx]);
        AddressablesManager.instance.ButtonGame(addressableIdx);
    }

    public void ButtonSubmitService()
    {
        string email = PlayerPrefs.GetString("Email", "Email");
        string body = $"{{\"user_email\":\"{email}\",\"message\":\"{inputService.text}\",\"contact_number\":{"Contact Number"}}}";

        StartCoroutine(IESupport(body));
        inputService.text = "";
    }

    public void ButtonRefer()
    {
        if (!isLoggedIn)
        {
            Msg.instance.DisplayMsg("Login to Refer", Color.white);
            return;
        }

        string referalCode = lobbyData.dataUser.referral_code;
        string link = $"{APIs.refral}/{referalCode}";
        string title = "100x Unlock the vault";
        string subject = "Refral for a game";

        string message = $"Want to experience the casino like never before? 100x Games offers epic thrills and HUGE payouts! Use my referal code  \"{referalCode}\"  to get discount/benfits when you signup. \nJoin the fun now! {link}";

        string filePath = Path.Combine(Application.temporaryCachePath, "shareimg.png");
        File.WriteAllBytes(filePath, textureShare.EncodeToPNG());

        new NativeShare().AddFile(filePath).SetText(title).SetSubject(subject).SetText(message).Share();
    }

    public void ButtonSubmitRefreal()
    {
        StartCoroutine(IESubmitRefral());
    }
}

[Serializable]
public class Game
{
    public string name;
    public int index;
}