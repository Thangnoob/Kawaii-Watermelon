using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject levelMapUI;
    private void Start()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;

        LevelMapManager.onLevelButtonClicked += LevelButtonClickedCallback;
    }

    

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
        
        LevelMapManager.onLevelButtonClicked -= LevelButtonClickedCallback;
    }

    private void GameStateChangedCallback(GameState state)
    {
        switch(state)
        {
            case GameState.Menu:
                SetMenu(); 
                break;
            case GameState.InGame: 
                SetInGame(); 
                break;
            case GameState.Gameover: 
                SetGameOver(); 
                break;
        }
    }

    private void SetMenu()
    {
        menuUI.SetActive(true);
        inGameUI.SetActive(false);
        gameOverUI.SetActive(false);
        settingPanel.SetActive(false);
        levelMapUI.SetActive(false);
        shopUI.SetActive(false);
    }

    private void SetInGame()
    {
        menuUI.SetActive(false);
        inGameUI.SetActive(true);
        gameOverUI.SetActive(false);
        settingPanel.SetActive(false);
        levelMapUI.SetActive(false);
        shopUI.SetActive(false);
    }

    private void SetGameOver()
    {
        menuUI.SetActive(false);
        inGameUI.SetActive(false);
        gameOverUI.SetActive(true);
        settingPanel.SetActive(false);
        levelMapUI.SetActive(false);
        shopUI.SetActive(false);
    }
    private void LevelButtonClickedCallback()
    {
        GameManager.Instance.SetGameState(GameState.InGame);
    }
    public void OpenSettingsPanel()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingPanel.SetActive(false);  
    }

    public void OpenShopUI()
    {
        shopUI.SetActive(true);
    }

    public void CloseShopUI()
    {
        shopUI.SetActive(false);
    }

    public void OpenLevelMapUI()
    {
        levelMapUI.SetActive(true);
    }

    public void CloseLevelMapUI()
    {
        levelMapUI.SetActive(false);
    }
}
