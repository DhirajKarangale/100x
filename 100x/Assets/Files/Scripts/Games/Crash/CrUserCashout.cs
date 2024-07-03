using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrUserCashout : MonoBehaviour
{
    [SerializeField] RocketController rocketController;
    [SerializeField] RectTransform rocket;
    [SerializeField] Image prefab;
    [SerializeField] float duration;
    [SerializeField] float speed;


    IEnumerator IEMove(RectTransform item)
    {
        float time = duration * 2;

        while (time > 0)
        {
            item.anchoredPosition -= new Vector2(0f, speed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
    }


    internal void CashOut(string userName)
    {
        Image item = Instantiate(prefab, rocket.position, Quaternion.identity);
        string data = userName + " " + rocketController.currScore.ToString("F2") + "x";
        item.GetComponentInChildren<TMPro.TMP_Text>().text = data;

        item.transform.SetParent(transform, false);
        item.GetComponent<RectTransform>().position = rocket.position;
        item.CrossFadeAlpha(0, duration, false);
        item.GetComponentInChildren<TMPro.TMP_Text>().CrossFadeAlpha(0, duration, false);
        StartCoroutine(IEMove(item.rectTransform));
    }


    internal void CashOut()
    {
        string userName = "User" + Random.Range(100, 1000).ToString();
        CashOut(userName);
    }
}