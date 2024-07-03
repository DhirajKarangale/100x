using UnityEngine;
using System.Collections;

public class CardsMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject objCard;
    [SerializeField] RectTransform[] posDec;


    private IEnumerator IEAnimate(RectTransform card, int dec)
    {
        float elapsedTime = 0;
        Vector3 targetSize = Vector3.one * 0.8f;

        while (elapsedTime < 1)
        {
            Vector3 newPosition = Vector3.Lerp(posDec[0].position, posDec[dec].position, elapsedTime);
            Vector2 newSize = Vector2.Lerp(Vector3.one, targetSize, elapsedTime);
            card.position = newPosition;
            card.localScale = newSize;
            elapsedTime += Time.deltaTime * speed;

            yield return null;
        }

        card.localScale = targetSize;
        card.position = posDec[dec].position;

        yield return new WaitForSecondsRealtime(0.15f);
        Destroy(card.gameObject);
    }

    internal void Animate(int dec)
    {
        RectTransform card = Instantiate(objCard).GetComponent<RectTransform>();
        card.SetParent(transform);
        card.position = posDec[0].position;
        card.localScale = Vector3.one;

        StartCoroutine(IEAnimate(card, dec));
    }
}