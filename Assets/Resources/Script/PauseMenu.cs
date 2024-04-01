using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public TextMeshProUGUI healthText;

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
    public static event Action resetHealth;

    public void RestartGame()
    {
        Debug.Log("reset health!!!");
        resetHealth?.Invoke();
        DamageableCharacter player = FindObjectOfType<DamageableCharacter>();
        if (player != null && player.CompareTag("Player"))
        {
            player.Health = 30;
            player.health = 30;
            HealthDisplay.UpdateHealth(30);

            healthText.text = "Health: " + 30;
        }
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
}