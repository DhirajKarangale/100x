using TMPro;
using UnityEngine;

public class AndarBaharUser : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] TMP_Text txtName;
    [SerializeField] TMP_Text txtAndar;
    [SerializeField] TMP_Text txtBahar;
    [SerializeField] UnityEngine.UI.Image image;
    [SerializeField] Sprite[] sprites;

    private int andar;
    private int bahar;


    internal void SetName(string userName)
    {
        txtName.text = userName.ToString();
        txtAndar.text = "0";
        txtBahar.text = "0";
        andar = 0;
        bahar = 0;
        image.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    internal void UpdateData(int andar, int bahar)
    {
        this.andar += andar;
        this.bahar += bahar;
        txtAndar.text = this.andar.ToString();
        txtBahar.text = this.bahar.ToString();
    }

    internal void PlayAnim(string anim)
    {
        animator.Play(anim);
    }
}