using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;    

    [Header(" Actions ")]
    public static Action<int> onScoreCalculated;
    public static Action<int> onBestScoreCalculated;

    [Header(" Settings ")]
    private int score = 0;
    private int bestScore = 0;
    [SerializeField] private int scoreMultiplyer;

    [Header(" Data ")]
    [SerializeField] private const string bestScoreKey = "bestScoreKey";

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);
        LoadData();
    }

    private void Start()
    {
        MergeManager.onMergeProcessed += MergeProgressdCallback;
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProgressdCallback;
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState state)
    {
        if (state == GameState.Gameover)
        {
            Debug.Log("ScoreManager: Trigger gameover");
            CalculateBestScore();
        }
    }

    private void CalculateBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            SaveData();
            onBestScoreCalculated?.Invoke(bestScore);
        }
    }

    private void MergeProgressdCallback(FruitType type, Vector2 vector)
    {
        int scoreToAdd = scoreMultiplyer * (int)type;
        score += scoreToAdd;

        onScoreCalculated?.Invoke(score);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(bestScoreKey, bestScore);
    }

    private void LoadData()
    {
        bestScore = PlayerPrefs.GetInt(bestScoreKey);   
    }

    public int GetBestScore()
    {
        LoadData();
        return bestScore;
    }
}
