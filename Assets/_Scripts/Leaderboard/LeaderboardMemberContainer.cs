using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardMemberContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image rankContainer;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void Configure(int rank, string name, int score)
    {
        rankContainer.color = GetRankColor(rank);
        rankText.text = rank.ToString();
        playerName.text = name;
        scoreText.text = score.ToString();
    }

    private Color GetRankColor(int rank)
    {
        Color rankColor = Color.gray;

        if (rank == 1)
        {
            rankColor = Color.yellow;
        }
        else if (rank == 2)
        {
            rankColor = Color.gray;
        }
        else if (rank == 3)
        {
            rankColor = new Color(0.8f, 0.5f, 0.2f); // Bronze color
        }

        return rankColor;
    }
}
