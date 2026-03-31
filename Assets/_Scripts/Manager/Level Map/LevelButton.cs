using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header(" Data ")]
    [SerializeField] private TextMeshProUGUI levelIndexText;
    [SerializeField] private Button levelButton;

    public void Start()
    {
        GetComponent<Image>().color = Random.ColorHSV(0f, 1f, .5f, 1f, .8f, 1f);
    }

    public void Configure(int levelIndex)
    {
        levelIndexText.text = levelIndex.ToString();
    }

    public void EnableButton()
    {
        levelButton.interactable = true;
    }

    public void DisableButton()
    {
        levelButton.interactable = false;
    }   

    public Button GetLevelButton() => levelButton;
}
