using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private SkinButton skinButton;
    [SerializeField] private Transform buttonSpawnArea;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI skinLabel;

    [Header(" Datas ")]
    [SerializeField] private SkinDataSO[] skinDataSOs;
    [SerializeField] private bool[] unlockSkinState;
    private int lastSelectedSkin;
    private const string skinButtonKey = "SkinButton_";
    private const string lastSelectedSkinKey = "LastSelectedSkin";
        
    [Header(" Actions ")]
    public static Action<SkinDataSO> onSkinSeleted;

    private void Awake()
    {
    }

    private void Start()
    {
        Initialize();

        LoadData();
    }

    private void Initialize()
    {
        purchaseButton.gameObject.SetActive(false);

        for (int i = 0; i < skinDataSOs.Length; i++)
        {
            SkinButton skinButtonInstance = Instantiate(skinButton, buttonSpawnArea);
            skinButtonInstance.Configure(
                skinDataSOs[i].GetSpawnablePrefabs()[0].GetComponent<Fruit>().GetFruitSprite());

            if (i == 0)
            {
                skinButtonInstance.ShowOutline();
            }

            int j = i;
            skinButtonInstance.GetSkinButton().onClick.AddListener(() => SkinButtonClickedCallback(j));
        }
    }

    private void SkinButtonClickedCallback(int skinButtonIndex)
    {
        lastSelectedSkin = skinButtonIndex;
        for (int i = 0;i < buttonSpawnArea.childCount; i++)
        {
            SkinButton currenSkinButton = buttonSpawnArea.GetChild(i).GetComponent<SkinButton>();
            if (i != skinButtonIndex)
                currenSkinButton.HideOutline();
            else
                currenSkinButton.ShowOutline();
        }

        if (isSkinUnlocked(skinButtonIndex))
        {
            onSkinSeleted?.Invoke(skinDataSOs[skinButtonIndex]);

            SaveLastSelectedSkin();
        }

        UpdateSkinLabel(skinButtonIndex);
        ManagePurchaseButtonVisibility(skinButtonIndex);
    }

    public void PurchaseButtonClickedCallback()
    {
        CoinManager.Instance.SpendCoins(skinDataSOs[lastSelectedSkin].GetPrice());

        unlockSkinState[lastSelectedSkin] = true;

        SaveData();

        SkinButtonClickedCallback(lastSelectedSkin);
    }

    private void ManagePurchaseButtonVisibility(int skinButtonIndex)
    {
        int skinPrice = skinDataSOs[skinButtonIndex].GetPrice();
        bool canPurchase = CoinManager.Instance.CanPurchase(skinPrice);
        purchaseButton.interactable = canPurchase;
        string text = skinPrice.ToString();
        
        purchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = text;


        purchaseButton.gameObject.SetActive(!unlockSkinState[skinButtonIndex]);
    }

    private bool isSkinUnlocked(int skinButtonIndex)
    {
        return unlockSkinState[skinButtonIndex];
    }
    
    private void UpdateSkinLabel(int skinButtonIndex)
    {
        skinLabel.text = skinDataSOs[skinButtonIndex].GetName();
    }

    private void LoadData()
    {
        unlockSkinState = new bool[skinDataSOs.Length];

        for (int i = 0; i < unlockSkinState.Length; i++)
        {
            int stateValue = PlayerPrefs.GetInt(skinButtonKey + i);

            if (i == 0)
            {
                stateValue = 1;
            }

            if (stateValue == 0)
                unlockSkinState[i] = false;
            else
                unlockSkinState[i] = true;
        }

        LoadLastSeletedSkin();
    }
    private void SaveData()
    {
        for (int i = 0; i < unlockSkinState.Length;i++)
        {
            int unlockState = unlockSkinState[i] ? 1 : 0;
            PlayerPrefs.SetInt(skinButtonKey + i, unlockState);
        }
    }

    private void LoadLastSeletedSkin()
    {
        int lastSeletedSkinIndex = PlayerPrefs.GetInt(lastSelectedSkinKey);
        SkinButtonClickedCallback(lastSeletedSkinIndex);
    }

    private void SaveLastSelectedSkin()
    {
        PlayerPrefs.SetInt(lastSelectedSkinKey, lastSelectedSkin);
    }
}
