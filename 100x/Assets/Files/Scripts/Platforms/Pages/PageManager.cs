using System;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] PageNumber[] pageNumbers;
    [SerializeField] RectTransform[] objPages;

    private int currPage;
    internal Action<int> OnPageClicked;


    private void Start()
    {
        Init();
        pageNumbers[0].ButtonClick();
    }

    private void Init()
    {
        currPage = 1;
        for (int i = 0; i < pageNumbers.Length; i++)
        {
            pageNumbers[i].Init((i + 1), (i + 1) == currPage);
            objPages[i].gameObject.SetActive(i == (currPage - 1));
        }
    }

    private void UpdatePageNumbers()
    {
        for (int i = 0; i < pageNumbers.Length; i++)
        {
            if ((i + 1) == currPage) pageNumbers[i].Select();
            else pageNumbers[i].Deselect();

            if (i == (currPage - 1))
            {
                objPages[i].gameObject.SetActive(true);
                scrollRect.content = objPages[i];
            }
            else
            {
                objPages[i].gameObject.SetActive(false);
            }
        }
    }


    public void ButtonPage(int idx)
    {
        currPage = idx;
        OnPageClicked?.Invoke(idx);
        UpdatePageNumbers();
    }

    public void ButtonNavigate(int amount)
    {
        currPage = Math.Clamp(currPage + amount, 1, pageNumbers.Length);
        ButtonPage(currPage);
    }
}