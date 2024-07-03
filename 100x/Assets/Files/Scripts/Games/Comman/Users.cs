using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Users : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject[] users;


    private void Start()
    {
        UpdateImages();
        UpdateNames(null);
    }


    internal void UpdateImages()
    {
        foreach (GameObject user in users)
        {
            Image icon = user.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            if (icon) icon.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }

    internal void UpdateNames(string names)
    {
        string[] userNames = null;

        if (!string.IsNullOrEmpty(names)) userNames = names.Split(',').Distinct().ToArray();

        for (int i = 0; i < 6; i++)
        {
            TMPro.TMP_Text txt = users[i].GetComponentInChildren<TMPro.TMP_Text>();
            string userName = "User" + Random.Range(111, 999).ToString();
            if (userNames != null && userNames.Length > 0 && i < userNames.Length) userName = userNames[i];
            txt.text = userName;
        }
    }
}