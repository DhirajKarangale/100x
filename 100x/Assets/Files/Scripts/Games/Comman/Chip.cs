using UnityEngine;

public class Chip : MonoBehaviour
{
    [SerializeField] internal string chipName;
    [SerializeField] internal Sprite sprite;
    [SerializeField] internal int betAmount;
    [SerializeField] BetManager betManager;

    internal void Reset()
    {
        transform.localScale = Vector3.one;
    }

    public void ButtonChip()
    {
        betManager.ButtonChip(this);
        transform.localScale = Vector3.one * 1.2f;
    }
}