using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理

public class WinMenu : MonoBehaviour
{
    public GameObject winMenuUI;

    

    public void GoRanking()
    {
        winMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1 );

    }

  
}