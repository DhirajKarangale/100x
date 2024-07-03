using TMPro;
using Google;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Init : MonoBehaviour
{
    [SerializeField] TMP_Text txtStatus;

    [Header("Login")]
    [SerializeField] TMP_InputField inputLoginPass;
    [SerializeField] TMP_InputField inputLoginEmail;

    [Header("Login")]
    [SerializeField] TMP_InputField inputSignUpName;
    [SerializeField] TMP_InputField inputSignUpRefral;
    [SerializeField] TMP_InputField inputSignUpPass;
    [SerializeField] TMP_InputField inputSignUpConfPass;
    [SerializeField] TMP_InputField inputSignUpEmail;
    [SerializeField] TMP_InputField inputSignUpMobile;

    private bool isOffline;
    private GoogleSignInConfiguration configuration;
    private readonly string webClientId = "549265281560-5rkacanj0cpvdqiomb2e7l7uqpg08mnr.apps.googleusercontent.com";


   
    private void OnEnable()
    {
        // PlayerPrefs.DeleteAll(); 
        // PlayerPrefs.SetString("AccessToken", "sg9WHpX1+SXYvdMXyzsp5Zyct7UJ1y+Uh6QCGBrZPuCIDDYKzs2nSpysug=="); 
        // PlayerPrefs.SetString("AccessToken", "qwBgEz6vmNlNR5XOEkJ7OmJjYFNxaNBE0rcSJHaxvI7uVfCZZg+A"); // DK
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckLogout();
        StartCoroutine(IECheckInternet());
    }


    private IEnumerator IECheckInternet()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            isOffline = true;
            txtStatus.color = Color.red;
            txtStatus.text = "Offline";
            Loading.instance.Disable();

            yield return new WaitForSecondsRealtime(2);

            StartCoroutine(IECheckInternet());
        }
        else
        {
            txtStatus.color = Color.green;
            txtStatus.text = "Online";
            isOffline = false;

            if (PlayerPrefs.HasKey("AccessToken")) LoadGame();
            else Loading.instance.Disable();

            yield break;
        }
    }

    private IEnumerator SignIn()
    {
        Msg.instance.DisplayMsg("Signing in...", Color.white);

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        var signIn = GoogleSignIn.DefaultInstance.SignIn();
        yield return new WaitUntil(() => signIn.IsCompleted || signIn.IsCompletedSuccessfully);

        string body = $"{{\"token\":\"{signIn.Result.IdToken}\",\"referral\":\"\"}}";
        StartCoroutine(IELogin(body, APIs.google));
    }

    private IEnumerator IELogin(string body, string api)
    {
        UnityWebRequest request = new UnityWebRequest(api, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        LoginRequest(request);
    }



    private void CheckLogout()
    {
        if (PlayerPrefs.GetInt("LogOut", 0) == 1)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("LogOut", 0);
            // GoogleSignIn.DefaultInstance.SignOut();
        }
    }

    private void LoginRequest(UnityWebRequest request)
    {
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            DataAuth data = JsonUtility.FromJson<DataAuth>(jsonData);

            PlayerPrefs.SetString("UserName", data.email.Split('@')[0]);
            PlayerPrefs.SetInt("Coins", data.coins);
            PlayerPrefs.SetString("AccessToken", data.access_token);
            PlayerPrefs.SetString("Email", data.email);
            PlayerPrefs.SetString("Name", data.name);
            // PlayerPrefs.SetString("ImageURL", data.picture.ToString());

            LoadGame();
            Msg.instance.DisplayMsg("Login Sucessful", Color.green);
        }
        else
        {
            // Msg.instance.DisplayMsg("Error while login, try again", Color.white);
            Msg.instance.DisplayMsg(request.error, Color.white);
            PlayerPrefs.DeleteAll();
            CheckLogout();
        }
    }

    private void LoadGame()
    {
        if (PlayerPrefs.HasKey("AccessToken") && !isOffline) PlayerPrefs.SetInt("LoggedIn", 1);
        string msg = "Getting towards Lobby...";
        if (Referral.isRefralSet) PlayerPrefs.SetInt("RefralActive", 1);

        Loading.instance.LoadLevel(1, 2, msg, 0, null);
    }



    public void ButtonLogin()
    {
        if (string.IsNullOrWhiteSpace(inputLoginEmail.text) || string.IsNullOrWhiteSpace(inputLoginPass.text))
        {
            Msg.instance.DisplayMsg("Enter valid email and password", Color.yellow);
            return;
        }

        string body = $"{{\"email\":\"{inputLoginEmail.text}\",\"password\":\"{inputLoginPass.text}\"}}";
        StartCoroutine(IELogin(body, APIs.login));
    }

    public void ButtonGoogle()
    {
        if (isOffline)
        {
            Msg.instance.DisplayMsg("You are offline", Color.yellow);
            return;
        }

        StartCoroutine(SignIn());
    }

    public void ButtonGuest()
    {
        inputSignUpName.text = "Dummy " + Random.Range(100, 1000);
        inputSignUpRefral.text = "";
        inputSignUpPass.text = "pass@123";
        inputSignUpConfPass.text = "pass@123";
        inputSignUpEmail.text = "dummy" + Random.Range(100, 9000) + "@mail.com";
        inputSignUpMobile.text = "0000000000";

        ButtonSignUp();
    }

    public void ButtonHelp()
    {
        string emailBody = "";
        string emailSubject = "Ticket from - " + PlayerPrefs.GetString("Name", "Your Name");
        emailSubject = System.Uri.EscapeUriString(emailSubject);
        Application.OpenURL("mailto:contact@100x.com" + "?subject=" + emailSubject + "&body=" + emailBody);
    }

    public void ButtonLink(string url)
    {
        Application.OpenURL(url);
    }

    public void ButtonSignUp()
    {
        if (string.IsNullOrWhiteSpace(inputSignUpName.text) || string.IsNullOrWhiteSpace(inputSignUpPass.text) ||
        string.IsNullOrWhiteSpace(inputSignUpConfPass.text) || string.IsNullOrWhiteSpace(inputSignUpEmail.text) ||
        string.IsNullOrWhiteSpace(inputSignUpMobile.text))
        {
            Msg.instance.DisplayMsg("Enter valid data", Color.yellow);
            return;
        }

        if (!inputSignUpPass.text.Equals(inputSignUpConfPass.text))
        {
            Msg.instance.DisplayMsg("Password and Confirm Password are not same", Color.yellow);
            return;
        }

        string body = "";

        if (string.IsNullOrEmpty(inputSignUpRefral.text)) body = $"{{\"name\":\"{inputSignUpName.text}\",\"email\":\"{inputSignUpEmail.text}\",\"password\":\"{inputSignUpPass.text}\",\"mobile_number\":\"{inputSignUpMobile.text}\"}}";
        else body = $"{{\"name\":\"{inputSignUpName.text}\",\"email\":\"{inputSignUpEmail.text}\",\"password\":\"{inputSignUpPass.text}\",\"mobile_number\":\"{inputSignUpMobile.text}\",\"referral\":\"{inputSignUpRefral.text}\"}}";

        StartCoroutine(IELogin(body, APIs.register));
    }
}