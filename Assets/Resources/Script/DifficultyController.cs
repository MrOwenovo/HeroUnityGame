using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class DifficultyController : MonoBehaviour
{
    public TextMeshProUGUI buttonText;

    private void Start()
    {
        UpdateButtonText(); 
    }

    public void ChangeDifficulty()
    {
        if (GameDifficulty.CurrentDifficulty == GameDifficulty.Difficulty.Easy)
        {
            GameDifficulty.CurrentDifficulty = GameDifficulty.Difficulty.Hard;
        }
        else
        {
            GameDifficulty.CurrentDifficulty = GameDifficulty.Difficulty.Easy;
        }

        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        if (GameDifficulty.CurrentDifficulty == GameDifficulty.Difficulty.Easy)
        {
            buttonText.text = "Change to Difficult";
        }
        else
        {
            buttonText.text = "Change to Easy";
        }
    }
}