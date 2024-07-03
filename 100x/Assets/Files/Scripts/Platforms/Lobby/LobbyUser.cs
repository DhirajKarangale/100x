using TMPro;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System;

public class LobbyUser : MonoBehaviour
{
    [SerializeField] Lobby lobby;
    [SerializeField] LobbyData lobbyData;
    [SerializeField] Sprite spriteDefault;
    [SerializeField] GameObject objRefer;

    [Header("Top")]
    [SerializeField] internal Image imgTop;
    [SerializeField] TMP_Text txtCoinTop;
    [SerializeField] TMP_Text txtNameTop;
    [SerializeField] TMP_Text txtEmailTop;

    [Header("Side")]
    [SerializeField] Image imgSide;
    [SerializeField] TMP_Text txtNameSide;
    [SerializeField] TMP_Text txtEmailSide;
    [SerializeField] TMP_Text txtCoins;
    [SerializeField] TMP_Text txtRewardCoin;
    [SerializeField] TMP_Text txtWinings;
    [SerializeField] TMP_Text txtMoneyAdded;
    [SerializeField] TMP_Text txtRefral;
    [SerializeField] TMP_Text txtContact;
    [SerializeField] TMP_Text txtLogin;

    [Header("Edit")]
    [SerializeField] TMP_InputField inputEditUserName;
    [SerializeField] TMP_InputField inputEditPass;
    [SerializeField] TMP_InputField inputEditContact;
    [SerializeField] TMP_InputField inputEditEmail;


    private bool isRefralActive;


    private void Start()
    {
        LobbyData.OnDataLoaded += SetData;
    }

    private void OnDestroy()
    {
        LobbyData.OnDataLoaded -= SetData;
    }


    private IEnumerator IEDownloadImage(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            imgTop.sprite = spriteDefault;
            imgSide.sprite = spriteDefault;

            yield break;
        }

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D userPicture = DownloadHandlerTexture.GetContent(www);

            imgTop.sprite = Sprite.Create(userPicture, new Rect(0, 0, userPicture.width, userPicture.height), new Vector2(userPicture.width / 2, userPicture.height / 2));
            imgSide.sprite = Sprite.Create(userPicture, new Rect(0, 0, userPicture.width, userPicture.height), new Vector2(userPicture.width / 2, userPicture.height / 2));

            byte[] textureBytes = userPicture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + "SavedImage", textureBytes);

            PlayerPrefs.SetString("SavedImageUrl", url);
        }

        www.Dispose();
    }

    private IEnumerator IEEdit()
    {
        string body = "";

        UnityWebRequest request = new UnityWebRequest(APIs.editUser, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("AccessToken"));
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            lobbyData.LoadData();
            Msg.instance.DisplayMsg("Edit sucess", Color.green);
        }
        else
        {
            Msg.instance.DisplayMsg("Edit failed, try again", Color.white);
        }
    }


    private void SetTexture(string imgUrl)
    {
        string savedImgUrl = PlayerPrefs.GetString("SavedImageUrl", "");

        if (!savedImgUrl.Equals(imgUrl) || !File.Exists(Application.persistentDataPath + "SavedImage"))
        {
            StartCoroutine(IEDownloadImage(imgUrl));
        }
        else
        {
            byte[] textureBytes = File.ReadAllBytes(Application.persistentDataPath + "SavedImage");
            Texture2D loadedTexture = new Texture2D(0, 0);
            loadedTexture.LoadImage(textureBytes);

            imgTop.sprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), new Vector2(loadedTexture.width / 2, loadedTexture.height / 2));
            imgSide.sprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), new Vector2(loadedTexture.width / 2, loadedTexture.height / 2));
        }
    }

    private void UpdateData(DataUser data)
    {
        PlayerPrefs.SetString("Name", data.name);
        PlayerPrefs.SetString("UserName", data.email.Split('@')[0]);
        PlayerPrefs.SetString("Email", data.email);
        PlayerPrefs.SetInt("Coins", data.coins);
        // PlayerPrefs.SetString("AccessToken", data.access_token);
    }

    private void SetData(DataUser dataUser)
    {
        UpdateData(dataUser);
        SetTexture(dataUser.picture);

        if (string.IsNullOrWhiteSpace(dataUser.referral_code)) dataUser.referral_code = "";
        if (string.IsNullOrWhiteSpace(dataUser.contact_number)) dataUser.contact_number = "";

        txtNameTop.text = dataUser.name;
        txtEmailTop.text = PlayerPrefs.GetString("UserName");

        txtNameSide.text = dataUser.name;
        txtEmailSide.text = PlayerPrefs.GetString("UserName");
        txtCoinTop.text = dataUser.coins.ToString();

        txtCoins.text = dataUser.coins.ToString();
        txtWinings.text = dataUser.total_winnings.ToString();
        txtRewardCoin.text = dataUser.reward_coins.ToString();
        txtMoneyAdded.text = dataUser.total_money_added.ToString();
        txtRefral.text = dataUser.referral_code.ToString();
        txtContact.text = dataUser.contact_number.ToString();

        inputEditUserName.text = PlayerPrefs.GetString("UserName");
        inputEditContact.text = dataUser.contact_number;
        inputEditEmail.text = dataUser.email;

        txtLogin.text = lobby.isLoggedIn ? "LogOut" : "LogIn";

        DateTime create = DateTime.Parse(dataUser.created_at.Split(' ')[0]);
        isRefralActive = (DateTime.Now - create).TotalDays <= 3;
    }


    public void ButtonEdit()
    {
        StartCoroutine(IEEdit());
    }

    public void ButtonRefralCode()
    {
        if (!isRefralActive)
        {
            Msg.instance.DisplayMsg("Refral expired", Color.white);
            return;
        }

        objRefer.SetActive(true);
    }
}