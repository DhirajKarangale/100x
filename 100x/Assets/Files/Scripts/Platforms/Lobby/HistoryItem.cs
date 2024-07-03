using TMPro;
using UnityEngine;

public class HistoryItem : MonoBehaviour
{
    [SerializeField] TMP_Text txtDate;
    [SerializeField] TMP_Text txtGame;
    [SerializeField] TMP_Text txtBidAmount;
    [SerializeField] TMP_Text txtBidOn;
    [SerializeField] TMP_Text txtWinner;


    private string FormatWinner(string inputText)
    {
        if (IsFloat(inputText))
        {
            float floatValue;
            if (float.TryParse(inputText, out floatValue))
            {
                return floatValue.ToString("F2") + "x";
            }
        }

        return inputText;
    }

    private bool IsFloat(string inputText)
    {
        float floatValue;
        return float.TryParse(inputText, out floatValue);
    }


    internal void SetData(DataHistoryItem data)
    {
        txtDate.text = data.created_at.Split(' ')[0].ToString();
        txtGame.text = data.game_type.ToString();
        txtBidAmount.text = data.bid_amount.ToString();
        // txtBidOn.text = data.bid_on.ToString();
        txtBidOn.text = "";

        txtWinner.text = FormatWinner(data.game_winner);
    }
}