using TMPro;
using System;
using System.IO;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class GameUserData : MonoBehaviour
{
    [SerializeField] TMP_Text txtCoins;
    [SerializeField] TMP_Text txtName;
    [SerializeField] TMP_Text txtEmail;
    [SerializeField] Image imgUser;
    internal int coins;


    private void Start()
    {
        GetUserData();
    }


    private IEnumerator IEGetUserData()
    {
        UnityWebRequest request = UnityWebRequest.Get(APIs.user);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("AccessToken"));

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            DataUser data = JsonUtility.FromJson<DataUser>(jsonData);
            SetData();
        }
        else
        {
            DataUser data = new DataUser();
            data.name = "User Name";
            data.coins = PlayerPrefs.GetInt("Coins", 0);
            data.total_winnings = 0;
            data.total_money_added = 0;
            data.email = "User Email";
            data.pan_number = "00000000";
            data.contact_number = "1234567890";
            data.picture = "";
            data.created_at = DateTime.Now.ToString();
            data.updated_at = DateTime.Now.ToString();
            data.access_token = "";

            SetData();
        }
    }


    private void SetData()
    {
        UpdateCoins(PlayerPrefs.GetInt("Coins", 0));
        txtName.text = PlayerPrefs.GetString("Name", "Name");
        txtEmail.text = PlayerPrefs.GetString("UserName", "user");

        if (File.Exists(Application.persistentDataPath + "SavedImage"))
        {
            byte[] textureBytes = File.ReadAllBytes(Application.persistentDataPath + "SavedImage");
            Texture2D loadedTexture = new Texture2D(0, 0);
            loadedTexture.LoadImage(textureBytes);

            imgUser.sprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), new Vector2(loadedTexture.width / 2, loadedTexture.height / 2)); ;
        }
    }


    internal void UpdateCoins(int coins)
    {
        this.coins = coins;
        txtCoins.text = coins.ToString();
    }

    internal void GetUserData()
    {
        StartCoroutine(IEGetUserData());
    }
}