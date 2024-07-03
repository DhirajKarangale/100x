using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DiceManager : MonoBehaviour
{
    [SerializeField] GameObject objAnim;
    [SerializeField] Animator animator;
    [SerializeField] Sprite[] sprites;

    [SerializeField] Transform container;
    [SerializeField] Image dice1Board;
    [SerializeField] Image dice2Board;
    [SerializeField] Image dice1Anim;
    [SerializeField] Image dice2Anim;

    private int side1, side2;
    private Dictionary<int, List<(int, int)>> sequencesMap;


    private void Start()
    {
        InitSequence();
        ResetData();
    }


    private IEnumerator IERotate(Image image, Sprite spriteRequired)
    {
        float timer = 0.02f;
        bool isFront = false;

        while (timer > 0)
        {
            if (!isFront)
            {
                for (float i = 0f; i <= 180f; i += 10)
                {
                    image.transform.rotation = Quaternion.Euler(0, i, 0);
                    if (i == 90) image.sprite = sprites[Random.Range(0, sprites.Length)];
                    yield return new WaitForSecondsRealtime(0.004f);
                }
            }
            else if (isFront)
            {
                for (float i = 180f; i >= 0f; i -= 10)
                {
                    image.transform.rotation = Quaternion.Euler(0, i, 0);
                    if (i == 90) image.sprite = sprites[Random.Range(0, sprites.Length)]; ;
                    yield return new WaitForSecondsRealtime(0.004f);
                }
            }
            isFront = !isFront;
            timer -= Time.deltaTime;
        }

        if (!isFront)
        {
            for (float i = 0f; i <= 180f; i += 10)
            {
                image.transform.rotation = Quaternion.Euler(0, i, 0);
                if (i == 90) image.sprite = spriteRequired;
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
        else if (isFront)
        {
            for (float i = 180f; i >= 0f; i -= 10)
            {
                image.transform.rotation = Quaternion.Euler(0, i, 0);
                if (i == 90) image.sprite = spriteRequired;
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

        image.transform.rotation = Quaternion.identity;
    }

    private IEnumerator IEBeginAnim()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        SoundManager soundManager = SoundManager.instance;
        if (soundManager) soundManager.Play(soundManager.clipDice);

        objAnim.SetActive(true);
        container.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(0.25f);

        StartCoroutine(IERotate(dice1Anim, sprites[side1 - 1]));
        StartCoroutine(IERotate(dice2Anim, sprites[side2 - 1]));

        yield return new WaitForSecondsRealtime(1.7f);

        dice1Board.gameObject.SetActive(true);
        dice2Board.gameObject.SetActive(true);
        dice1Board.sprite = sprites[side1 - 1];
        dice2Board.sprite = sprites[side2 - 1];

        yield return new WaitForSecondsRealtime(0.1f);

        objAnim.SetActive(false);
    }

    private IEnumerator IEBegin()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        animator.Play("Play");
    }

    private IEnumerator IEEnd()
    {
        yield return new WaitForSecondsRealtime(8f);
        GameManager.OnFinishWinAnim?.Invoke();

        SoundManager soundManager = SoundManager.instance;
        if (soundManager) soundManager.Stop();
    }


    private void InitSequence()
    {
        sequencesMap = new Dictionary<int, List<(int, int)>>();

        for (int i = 1; i <= 6; i++)
        {
            for (int j = 1; j <= 6; j++)
            {
                int sum = i + j;
                if (!sequencesMap.ContainsKey(sum)) sequencesMap[sum] = new List<(int, int)>();
                sequencesMap[sum].Add((i, j));
            }
        }
    }

    private (int, int) GetSequenceForSum(int targetSum)
    {
        if (sequencesMap.ContainsKey(targetSum))
        {
            List<(int, int)> sequences = sequencesMap[targetSum];
            int randomIndex = Random.Range(0, sequences.Count);
            return sequences[randomIndex];
        }
        else
        {
            return (1, 1);
        }
    }

    private (int, int) GetSides(int tow, int seven, int eight)
    {
        int num = 1;

        if (tow == 1) num = Random.Range(2, 7);
        else if (seven == 1) num = 7;
        else if (eight == 1) num = Random.Range(8, 13);

        return GetSequenceForSum(num);
    }


    internal void ResetData()
    {
        StopAllCoroutines();

        animator.Play("Idle");
        objAnim.SetActive(false);
        container.gameObject.SetActive(true);
        dice1Board.gameObject.SetActive(false);
        dice2Board.gameObject.SetActive(false);

        dice1Board.sprite = sprites[0];
        dice2Board.sprite = sprites[0];
        dice1Anim.sprite = sprites[0];
        dice2Anim.sprite = sprites[0];
    }

    internal void Begin(int two, int seven, int eight)
    {
        StopAllCoroutines();
        (side1, side2) = GetSides(two, seven, eight);
        StartCoroutine(IEBegin());
        StartCoroutine(IEEnd());
    }


    public void AnimBegin()
    {
        StartCoroutine(IEBeginAnim());
    }
}