using UnityEngine;
using UnityEngine.UI;

public class LobbySetting : MonoBehaviour
{
    [SerializeField] Slider sliderMusic;
    [SerializeField] Toggle toggleVibration;
    [SerializeField] Toggle toggleNotification;
    [SerializeField] TMPro.TMP_Text txtMusic;


    private void Start()
    {
        UpdateUI();
    }


    private void UpdateUI()
    {
        txtMusic.text = $"Music {PlayerPrefs.GetInt("CurrMusic", 0)}";
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
        toggleVibration.isOn = PlayerPrefs.GetInt("Vibration", 0) == 1;
        toggleNotification.isOn = PlayerPrefs.GetInt("Notification", 0) == 1;
    }


    public void ButtonMusic()
    {
        SoundManager soundManager = SoundManager.instance;
        int currMusic = PlayerPrefs.GetInt("CurrMusic", 0);
        currMusic = (currMusic + 1) % soundManager.clipsbg.Length;
        PlayerPrefs.SetInt("CurrMusic", currMusic);
        soundManager.UpdateBG(currMusic);
        UpdateUI();
    }

    public void SliderBG(float val)
    {
        SoundManager soundManager = SoundManager.instance;
        PlayerPrefs.SetFloat("MusicVolume", val);
        UpdateUI();
        soundManager.UpdateBGVolume(val);
    }

    public void ButtonNotification(bool isOn)
    {
        PlayerPrefs.SetInt("Notification", isOn ? 1 : 0);
        UpdateUI();
    }

    public void ButtonVibration(bool isOn)
    {
        PlayerPrefs.SetInt("Vibration", isOn ? 1 : 0);
        UpdateUI();
    }
}