using TMPro;
using UnityEngine;

public class CrNumbers : MonoBehaviour
{
    [SerializeField] Transform contentVertical;
    [SerializeField] Transform contentHorrizontal;
    [SerializeField] TMP_Text prefabVertical;


    private void Start()
    {
        Spawn();
    }


    private void Spawn()
    {
        VerticalNumbers();
        HorrizontalNumbers();
    }


    private void VerticalNumbers()
    {
        bool isLine = false;

        float startValue = 0.0f;
        float endValue = 25.0f;
        float step = 0.1f;

        for (float value = endValue; value >= startValue - step; value -= step)
        {
            string text = value.ToString("F1") + 'x';

            TMP_Text newText = Instantiate(prefabVertical, contentVertical);
            newText.text = isLine ? "-" : text;
            newText.alignment = TextAlignmentOptions.BottomRight;
            newText.name = text;
            isLine = !isLine;
        }
    }

    private void HorrizontalNumbers()
    {
        bool isLine = false;

        float startValue = 0.0f;
        float endValue = 40.0f;
        // float step = 0.5f;
        float step = 1.0f;

        for (float value = startValue; value <= endValue; value += step)
        {
            string text = value.ToString("F0") + 's';

            TMP_Text newText = Instantiate(prefabVertical, contentHorrizontal);
            // newText.text = isLine ? "|" : text;
            newText.text = text;
            newText.alignment = TextAlignmentOptions.TopLeft;
            newText.name = text;
            isLine = !isLine;
        }
    }
}