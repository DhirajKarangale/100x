using UnityEngine;

public class Referral : MonoBehaviour
{
    [SerializeField] Init init;
    [SerializeField] GameObject objLogin;
    [SerializeField] GameObject objSignUp;
    [SerializeField] TMPro.TMP_InputField inputRefral;
    internal static bool isRefralSet;


    private void Start()
    {
        Loading.instance.Active();
        if (!string.IsNullOrEmpty(Application.absoluteURL)) LinkFound(Application.absoluteURL);
        else init.enabled = true;
    }

    private void LinkFound(string url)
    {
        if (url.Contains("referral="))
        {
            PlayerPrefs.DeleteAll();
            int index = url.IndexOf("referral=") + "referral=".Length;
            string referralCode = url.Substring(index);
            int ampersandIndex = referralCode.IndexOf('&');
            if (ampersandIndex != -1) referralCode = referralCode.Substring(0, ampersandIndex);

            if (!string.IsNullOrEmpty(referralCode) && PlayerPrefs.GetInt("RefralActive", 0) != 1) // PlayerPrefs.GetInt("LoggedIn", 0)
            {
                Msg.instance.DisplayMsg("Activating refral code", Color.green);
                objLogin.SetActive(false);
                objSignUp.SetActive(true);
                inputRefral.text = referralCode;
                Loading.instance.Disable();

                isRefralSet = true;
            }
        }

        init.enabled = true;
    }
}