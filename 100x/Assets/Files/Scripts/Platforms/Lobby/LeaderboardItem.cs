using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] TMP_Text txtName;
    [SerializeField] TMP_Text txtRank;
    [SerializeField] TMP_Text txtWinnings;

    internal void SetData(int rank, DataLeaderboard item)
    {
        txtRank.text = $"#{rank}";
        txtName.text = item.email.Split('@')[0];
        txtWinnings.text = item.total_winnings.ToString();
    }
}