using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    [Header(" Datas ")]
    private int coins;
    private const string coinKey = "Coins";

    [Header(" Actions ")]
    public static Action onCoinsChanged;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        LoadData();
        UpdateCoinText();
        MergeManager.onMergeProcessed += MergeProgressedCallback;
    }


    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProgressedCallback;
    }

    private void MergeProgressedCallback(FruitType type, Vector2 spawnPos)
    {
        int coinAmount = (int)type;

        AddCoins(coinAmount);

        UpdateCoinText();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        coins = Mathf.Max(coins, 0); 
        SaveData();
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            SaveData();
            UpdateCoinText();
            return true;
        }
        else
        {
            Debug.Log("Not enough coins!");
            return false;
        }
    }

    public bool CanPurchase(int price)
    {
        return coins >= price;
    }

    private void UpdateCoinText()
    {
        CoinText[] coinTexts = Resources.FindObjectsOfTypeAll<CoinText>();

        for (int i = 0;i < coinTexts.Length; i++)
        {
            coinTexts[i].UpdateCoinText(coins.ToString());
        }

        onCoinsChanged?.Invoke();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(coinKey, coins);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt(coinKey, 0);
    }
}
