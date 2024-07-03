using UnityEngine;

public class PageNumber : MonoBehaviour
{
    [SerializeField] PageManager pageManager;
    private int idx;

    [Header("UI")]
    [SerializeField] TMPro.TMP_Text txt;
    [SerializeField] UnityEngine.UI.Image image;
    [SerializeField] UnityEngine.UI.Button button;

    [Header("Colors")]
    [SerializeField] Color colorBGSelect;
    [SerializeField] Color colorBGDeselect;
    [SerializeField] Color colorTxtSelect;
    [SerializeField] Color colorTxtDeselect;


    internal void Init(int page, bool isSelect)
    {
        idx = page;
        txt.text = page.ToString();
        if(isSelect) Select();
        else Deselect();
    }

    internal void Deselect()
    {
        button.interactable = true;
        image.color = colorBGDeselect;
        txt.color = colorTxtDeselect;
    }

    internal void Select()
    {
        button.interactable = false;
        image.color = colorBGSelect;
        txt.color = colorTxtSelect;
    }


    public void ButtonClick()
    {
        pageManager.ButtonPage(idx);
    }
}