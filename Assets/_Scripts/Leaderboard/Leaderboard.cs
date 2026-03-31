using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI leaderboardText;

    [Header("Leaderboard Settings")]
    [SerializeField] private string leaderboardKey = "default_leaderboard";

    [Header(" Actions ")]
    public static Action<LootLockerLeaderboardMember[]> onLeaderboardFetched;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        PlayerAuthenticate.onPlayerAuthenticated += RefreshLeaderboard;
        PlayerLeaderboard.onPlayerNameChanged += RefreshLeaderboard;
    }

    private void OnDestroy()
    {
        PlayerAuthenticate.onPlayerAuthenticated -= RefreshLeaderboard;
        PlayerLeaderboard.onPlayerNameChanged -= RefreshLeaderboard;    
    }

    public void SubmitScore(string memberId, int score)
    {
        StartCoroutine(SubmitScoresCoroutine(memberId, score));
    }

    IEnumerator SubmitScoresCoroutine(string memberId, int score)
    {
        bool done = false;

        LootLockerSDKManager.SubmitScore(memberId, score, leaderboardKey,(response) =>
        {
            if (response.success)
            {
                Debug.Log("Score submitted successfully!");

                done = true;
            }
            else
            {
                Debug.LogError("Failed to submit score: " + response.errorData);

                done = true;
            }
        });

        yield return new WaitWhile(()=>done);
    }

    private void FectchScores()
    {
        StartCoroutine(FetchScoresCoroutine());
    }

    IEnumerator FetchScoresCoroutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Scores fetched successfully!");
                onLeaderboardFetched?.Invoke(response.items);

                done = true;
            }
            else
            {
                Debug.LogError("Failed to fetch scores: " + response.errorData);
                done = true;
            }
        });
        yield return new WaitUntil(() => done);
    }

    public void RefreshLeaderboard()
    {
        FectchScores();
    }
}
