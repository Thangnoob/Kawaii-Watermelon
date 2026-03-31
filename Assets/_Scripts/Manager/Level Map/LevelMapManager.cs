using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMapManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform mapContent;
    [SerializeField] private RectTransform[] levelButtonParents;
    [SerializeField] private LevelButton levelButtonPrefab;
    [SerializeField] private LevelDataSO[] levelDataSOs;

    [Header(" Actions ")]
    public static Action onLevelButtonClicked;
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        mapContent.anchoredPosition = Vector2.up * 1920 * (mapContent.childCount -1);

       CreateLevelButtons();
    }

    private void CreateLevelButtons() {
        for (int i = 0; i < levelButtonParents.Length; i++)
        {
            if (levelDataSOs[i] != null)
            {
                CreateLevelButton(i, levelButtonParents[i]);

            }
        }
    }

    private void CreateLevelButton(int levelIndex, Transform levelButtonParent)
    {
        LevelButton levelButton = Instantiate(levelButtonPrefab, levelButtonParent);
        levelButton.Configure(levelIndex + 1);

        SetLevelButtonInteraction(levelIndex, levelButton.GetLevelButton());
    }

    private void SetLevelButtonInteraction(int levelIndex, Button levelButton)
    {
        int bestScore = ScoreManager.Instance.GetBestScore();
        int hightScoreRequired = levelDataSOs[levelIndex].HightScoreRequired;
        if (bestScore < hightScoreRequired)
            {
            levelButton.GetComponent<LevelButton>().DisableButton();
        }
        else
        {
            levelButton.GetComponent<LevelButton>().EnableButton();
            levelButton.onClick.AddListener(() => OnLevelButtonClicked(levelIndex));

        }
    }

    private void OnLevelButtonClicked(int levelIndex)
    {
        while (transform.childCount > 0)
        {
            Transform t = transform.GetChild(0);
            t.SetParent(null);      
            Destroy(t.gameObject);
        }

        Instantiate(levelDataSOs[levelIndex].LevelMapPrefab, transform);
        
        onLevelButtonClicked?.Invoke();
    }
}
