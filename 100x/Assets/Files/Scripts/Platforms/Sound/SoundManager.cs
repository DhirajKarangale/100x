using UnityEngine;
using CandyCoded.HapticFeedback;

public class SoundManager : PersistentSingleton<SoundManager>
{
    [Header("Audio Source")]
    [SerializeField] AudioSource audioMusic;
    [SerializeField] AudioSource audioSFX;

    [Header("Clips")]
    [SerializeField] internal AudioClip[] clipsbg;
    [SerializeField] internal AudioClip clipCoin;
    [SerializeField] internal AudioClip clipDice;
    [SerializeField] internal AudioClip clipTiger;
    [SerializeField] internal AudioClip clipDragon;
    [SerializeField] internal AudioClip clipButton;
    [SerializeField] internal AudioClip clipButtonBig;


    private void Start()
    {
        UpdateBG(PlayerPrefs.GetInt("CurrMusic", 0));
        UpdateBGVolume(PlayerPrefs.GetFloat("MusicVolume", 0.3f));
    }


    internal void Stop()
    {
        audioSFX.Stop();
    }

    internal void Play(AudioClip clip, int volume = 1)
    {
        audioSFX.volume = volume;
        audioSFX.PlayOneShot(clip);
    }

    internal void SoundButton()
    {
        Play(clipButton);
    }

    internal void SoundButtonBig()
    {
        Play(clipButtonBig);
    }

    internal void UpdateBGVolume(float amount)
    {
        audioMusic.volume = amount;
    }

    internal void UpdateBG(int idx)
    {
        if (audioMusic.clip == clipsbg[idx]) return;

        audioMusic.clip = clipsbg[idx];
        audioMusic.Play();
    }

    internal void Vibration(int feedBack)
    {
        if (PlayerPrefs.GetInt("Vibration", 0) != 1) return;

        if (feedBack == 0) HapticFeedback.LightFeedback();
        else if (feedBack == 1) HapticFeedback.MediumFeedback();
        else if (feedBack == 2) HapticFeedback.HeavyFeedback();
    }
}