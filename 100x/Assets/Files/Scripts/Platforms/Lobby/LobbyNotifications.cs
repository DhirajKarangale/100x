using UnityEngine;
using System.Collections;

public class LobbyNotifications : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text txt;
    private string[] msgs = { "Dragon Won recent game", "Last game is Tie", "Tiger Won recent game", "Tiger beat the Dragon in last game", "Got Resut 2", "7 is Domenating", "2-6 won this match", "8-12 is in Top", "Recent rocket bust at 1.5x", "Recent rocket bust at 6.3x", "Rocket reach 8.3x", "Rocket burst at 4.5x" };

    private void Start()
    {
        StartCoroutine(IEShow());
    }

    private IEnumerator IEShow()
    {
        while (true)
        {
            string user = "User" + Random.Range(1, 1000);
            txt.text = msgs[Random.Range(0, msgs.Length)];
            yield return new WaitForSecondsRealtime(5);
        }
    }
}