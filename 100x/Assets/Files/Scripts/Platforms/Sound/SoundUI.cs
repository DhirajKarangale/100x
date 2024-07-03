using UnityEngine;

public class SoundUI : MonoBehaviour
{
    public void Button()
    {
        SoundManager soundManager = SoundManager.instance;
        if (soundManager) soundManager.SoundButton();
    }

    public void ButtonBig()
    {
        SoundManager soundManager = SoundManager.instance;
        if (soundManager) soundManager.SoundButtonBig();
    }
}