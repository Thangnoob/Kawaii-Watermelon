using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMapManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform mapContent;
    [SerializeField] private RectTransform[] levelButtonParents;
    [SerializeField] private LevelButton levelButtonPrefab;

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
           CreateLevelButton(i, levelButtonParents[i]);
        }
    }

    private void CreateLevelButton(int levelIndex, Transform levelButtonParent)
    {
        LevelButton levelButton = Instantiate(levelButtonPrefab, levelButtonParent);
        levelButton.Configure(levelIndex + 1);
    }
}
