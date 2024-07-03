using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardsManager : MonoBehaviour
{
    [SerializeField] CardsMove cardsMove;
    [SerializeField] Sprite[] spritesCards;

    [SerializeField] Sprite spriteBack;
    [SerializeField] Image imgFirst;
    [SerializeField] Image imgCenter;
    [SerializeField] Image imgSecond;
    private int centerImage;


    private IEnumerator IERotate(Image image, Sprite spriteRequired)
    {
        image.color = Color.white;
        float timer = 0.01f;
        bool isFront = false;

        while (timer > 0)
        {
            if (!isFront)
            {
                for (float i = 0f; i <= 180f; i += 10)
                {
                    image.transform.rotation = Quaternion.Euler(0, i, 0);
                    if (i == 90) image.sprite = spritesCards[Random.Range(0, spritesCards.Length)];
                    yield return new WaitForSecondsRealtime(0.0075f);
                }
            }
            else if (isFront)
            {
                for (float i = 180f; i >= 0f; i -= 10)
                {
                    image.transform.rotation = Quaternion.Euler(0, i, 0);
                    if (i == 90) image.sprite = spriteBack;
                    yield return new WaitForSecondsRealtime(0.0075f);
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

    private IEnumerator IEStartDVT(int win)
    {
        yield return new WaitForSecondsRealtime(0.25f);

        if (win == 0) // Tie
        {
            int card = Random.Range(0, spritesCards.Length);
            StartCoroutine(IERotate(imgFirst, spritesCards[card]));
            StartCoroutine(IERotate(imgSecond, spritesCards[card]));
        }
        else
        {
            int[] suit = GetSuit();
            int st = suit[0];
            int en = suit[1];

            int winCard = Random.Range(st + 2, en + 1);
            int loosCard = Random.Range(st, winCard);

            if (win == 1) // Dragon
            {
                StartCoroutine(IERotate(imgFirst, spritesCards[winCard]));
                StartCoroutine(IERotate(imgSecond, spritesCards[loosCard]));
            }
            else if (win == 2)
            {
                StartCoroutine(IERotate(imgFirst, spritesCards[loosCard]));
                StartCoroutine(IERotate(imgSecond, spritesCards[winCard]));
            }
        }
    }

    private IEnumerator IEStartAndarBahar(int win)
    {
        yield return new WaitForSecondsRealtime(0.25f);

        int card = Random.Range(0, spritesCards.Length);
        while (card == centerImage) card = Random.Range(0, spritesCards.Length);

        cardsMove.Animate(1);
        cardsMove.Animate(2);
        yield return new WaitForSecondsRealtime(0.5f);

        if (win == 0) // Andar
        {
            StartCoroutine(IERotate(imgFirst, spritesCards[centerImage]));
            StartCoroutine(IERotate(imgSecond, spritesCards[card]));
        }
        else // Bahar
        {
            StartCoroutine(IERotate(imgFirst, spritesCards[card]));
            StartCoroutine(IERotate(imgSecond, spritesCards[centerImage]));
        }
    }

    private IEnumerator IEMidAndarBahar()
    {
        yield return new WaitForSecondsRealtime(0.25f);

        Color emptyColor = Color.white;
        emptyColor.a = 0;
        if (imgCenter.sprite == null || imgCenter.color == emptyColor)
        {
            int[] suit = GetSuit();
            centerImage = Random.Range(suit[0], suit[1]);
            StartCoroutine(IERotate(imgCenter, spritesCards[centerImage]));
        }

        int card1 = 0;
        int card2 = 1;

        while (card1 == centerImage) card1 = Random.Range(0, spritesCards.Length);
        while (card2 == centerImage) card2 = Random.Range(0, spritesCards.Length);

        cardsMove.Animate(1);
        cardsMove.Animate(2);
        yield return new WaitForSecondsRealtime(0.5f);

        StartCoroutine(IERotate(imgFirst, spritesCards[card1]));
        StartCoroutine(IERotate(imgSecond, spritesCards[card2]));
    }

    private IEnumerator IEEnd()
    {
        yield return new WaitForSecondsRealtime(3f);
        GameManager.OnFinishWinAnim?.Invoke();
        centerImage = -1;
    }


    private int[] GetSuit()
    {
        int st = 0;
        int en = 12;

        int suit = Random.Range(0, 5);

        if (suit == 0)
        {
            st = 0;
            en = 12;
        }
        else if (suit == 1)
        {
            st = 13;
            en = 25;
        }
        else if (suit == 2)
        {
            st = 26;
            en = 38;
        }
        else if (suit == 3)
        {
            st = 39;
            en = 51;
        }

        int[] suitSelected = { st, en };

        return suitSelected;
    }


    internal void ResetData()
    {
        StopAllCoroutines();
        imgFirst.sprite = spriteBack;
        imgSecond.sprite = spriteBack;
    }

    internal void DVTStart(int win)
    {
        StopAllCoroutines();
        StartCoroutine(IEStartDVT(win));
        StartCoroutine(IEEnd());
    }

    internal void AndarBaharStart(int win)
    {
        StopAllCoroutines();
        StartCoroutine(IEStartAndarBahar(win));
        StartCoroutine(IEEnd());
    }

    internal void AndarBaharMid()
    {
        StopAllCoroutines();
        StartCoroutine(IEMidAndarBahar());
    }

    internal void AndarBaharNewGame()
    {
        StopAllCoroutines();

        Color color = Color.white;
        color.a = 0;
        imgFirst.color = color;
        imgSecond.color = color;

        int[] suit = GetSuit();
        centerImage = Random.Range(suit[0], suit[1]);
        StartCoroutine(IERotate(imgCenter, spritesCards[centerImage]));
    }
}