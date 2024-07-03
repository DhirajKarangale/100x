using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class HorseManager : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] float speedMin;
    [SerializeField] float speedMax;
    [SerializeField] float speedWin;

    [Header("Pos")]
    [SerializeField] float stPosHorse;
    [SerializeField] float enPosHorse;
    [SerializeField] float stPosPath;
    [SerializeField] float enPosPath;

    [Header("Refrences")]
    [SerializeField] RectTransform path;
    [SerializeField] RectTransform[] horses;

    [Header("Win")]
    [SerializeField] Animator[] animHorse;
    [SerializeField] GameObject objWin;
    [SerializeField] Image imgHorse;
    [SerializeField] TMP_Text txtHorse;
    [SerializeField] TMP_Text txtReward;
    [SerializeField] Sprite[] spritesHorses;
    [SerializeField] string[] rewards;
    private int win;


    private IEnumerator IEMoveHorse(RectTransform horse, float speed)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = new Vector3(stPosHorse, horse.anchoredPosition.y, 0);
        Vector3 targetPos = new Vector3(enPosHorse, horse.anchoredPosition.y, 0);

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * speed;
            horse.anchoredPosition = Vector3.Lerp(startingPos, targetPos, elapsedTime);
            if (horse.anchoredPosition.x >= enPosHorse)
            {
                StopAllCoroutines();
                StartCoroutine(IEMovePath());
            }

            yield return null;
        }

        horse.anchoredPosition = targetPos;
    }

    private IEnumerator IEMovePath()
    {
        float elapsedTime = 0f;
        Vector3 startingPos = new Vector3(stPosPath, path.anchoredPosition.y, 0);
        Vector3 targetPos = new Vector3(enPosPath, path.anchoredPosition.y, 0);

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * speedMax;
            path.anchoredPosition = Vector3.Lerp(startingPos, targetPos, elapsedTime);

            yield return null;
        }

        path.anchoredPosition = targetPos;
        End();
    }


    private void End()
    {
        foreach (Animator anim in animHorse)
        {
            anim.Play("Idle");
        }
        
        StopAllCoroutines();
        StartCoroutine(IEEnd());
    }

    private void GameOver()
    {   
        txtHorse.text = (win + 1).ToString();
        txtReward.text = rewards[win];
        imgHorse.sprite = spritesHorses[win];

        objWin.SetActive(true);
    }

    private IEnumerator IEEnd()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        GameManager.OnFinishWinAnim?.Invoke();

        yield return new WaitForSecondsRealtime(0.5f);
        GameOver();
    }


    internal void ResetData()
    {
        StopAllCoroutines();
        objWin.SetActive(false);

        path.anchoredPosition = new Vector3(stPosPath, path.anchoredPosition.y, 0);
        foreach (RectTransform horse in horses)
        {
            horse.anchoredPosition = new Vector3(stPosHorse, horse.anchoredPosition.y, 0);
        }

        foreach (Animator anim in animHorse)
        {
            anim.Play("Idle");
        }
    }

    internal void Begin(int win)
    {
        this.win = win;
        ResetData();

        for (int i = 0; i < horses.Length; i++)
        {
            float speed = (i != win) ? Random.Range(speedMin, speedMax) : speedWin;
            StartCoroutine(IEMoveHorse(horses[i], speed));
        }

        foreach (Animator anim in animHorse)
        {
            anim.Play("Play");
        }
    }
}