using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Button blastButton;

    [Header(" Datas ")]
    [SerializeField] private int blastPrice;

    private void Awake()
    {
        blastButton.onClick.AddListener(BlastPowerupCallback);
        CoinManager.onCoinsChanged += CoinChangedCallback;
    }

    private void Start()
    {
        LoadData();
    }
    private void OnDestroy()
    {
        CoinManager.onCoinsChanged -= CoinChangedCallback;
    }

    public void BlastPowerupCallback()
    {
        Debug.Log("Blast Powerup Activated!");
        Fruit[] smallFruits = FruitManager.Instance.GetSmallFruitForBlast();
        
        if (smallFruits == null || smallFruits.Length == 0)
        {
            Debug.Log("No small fruits to blast!");
            return;
        }

        for (int i = 0;  i < smallFruits.Length; i++)
        {
            smallFruits[i].Merge();
        }

        if (CoinManager.Instance.SpendCoins(blastPrice))
        {
            Debug.Log("Blast Powerup Purchased!");
        }
         else
        {
            Debug.Log("Not enough coins for Blast Powerup!");
        }
    }

    private void CoinChangedCallback()
    {
        UpdateBlastButtonVisibility();
    }

    private void UpdateBlastButtonVisibility()
    {
        blastButton.interactable = CoinManager.Instance.CanPurchase(blastPrice);
    }

    private void LoadData()
    {
        blastButton.GetComponentsInChildren<TextMeshProUGUI>()[1].text = blastPrice.ToString();
    }
}
