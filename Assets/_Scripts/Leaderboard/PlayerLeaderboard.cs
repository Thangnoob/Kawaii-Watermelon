using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaderboard : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private PlayerAuthenticate playerAuthenticate;

    [Header(" Actions ")]
    public static Action onPlayerNameChanged;

    private void Start()
    {
        ScoreManager.onBestScoreCalculated += SubmitPlayerScoreToLeaderboard;
    }

    private void OnDestroy()
    {
        ScoreManager.onBestScoreCalculated -= SubmitPlayerScoreToLeaderboard;
    }

    public void SubmitPlayerScoreToLeaderboard(int score)
    {
        string playerId = playerAuthenticate.PlayerId;
        Leaderboard.Instance.SubmitScore(playerId, score);
    }

    public void SetPlayerName(string playerName)
    {
        StartCoroutine(SetPlayerNameCoroutine(playerName));
    }

    IEnumerator SetPlayerNameCoroutine(string playerName)
    {
        bool done = false;
        LootLockerSDKManager.SetPlayerName(playerName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Player name set successfully!");
                onPlayerNameChanged?.Invoke();
            }
            else
            {
                Debug.LogError("Failed to set player name: " + response.errorData);
            }
            done = true;
        });
        yield return new WaitUntil(() => !done);
    }
}
