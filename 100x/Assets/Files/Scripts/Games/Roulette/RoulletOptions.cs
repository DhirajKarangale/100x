using UnityEngine;

public class RoulletOptions : MonoBehaviour
{
    public void ButtonExit()
    {
        string msg = "Leaving Game...";
        Loading.instance.LoadLevel(1, 3, msg, 0, null);
    }
}