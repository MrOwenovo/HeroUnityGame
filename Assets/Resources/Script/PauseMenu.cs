using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // 暂停游戏
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // 恢复游戏
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // 确保时间正常运行
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 重载当前场景
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // 确保时间正常运行
        SceneManager.LoadScene("MainMenu"); // 加载主菜单场景，假设场景名为"MainMenu"
    }
}