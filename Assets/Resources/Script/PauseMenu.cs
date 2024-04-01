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
        Time.timeScale = 1f;
        // 注册场景加载完成后的回调
        SceneManager.sceneLoaded += OnSceneLoaded;
        // 重载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // 场景加载完成后执行的回调方法
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 找到玩家并重置健康状态
        DamageableCharacter player = FindObjectOfType<DamageableCharacter>();
        if (player != null && player.CompareTag("Player"))
        {
            player.Health = 30;
            player.health = 30;
            HealthDisplay.UpdateHealth(30);
            healthText.text = "Health: " + 30;
        }

        // 移除事件监听，避免重复调用
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
}