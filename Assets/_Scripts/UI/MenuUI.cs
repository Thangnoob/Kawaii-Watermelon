using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI bestScoreText;
        
    private void Start()
    {
        bestScoreText.text = ScoreManager.Instance.GetBestScore().ToString();
    }
}
