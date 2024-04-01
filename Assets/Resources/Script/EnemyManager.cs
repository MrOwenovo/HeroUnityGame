using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public GameObject meleeEnemyPrefab;
    public int maxEnemies = 10;
    private List<GameObject> enemies = new List<GameObject>();
    public float spawnRadius = 9.5f;
    public LayerMask obstacleLayer;
    private float gameTimer = 7f;
    
    public GameObject rangedEnemyPrefab;
    private int currentMeleeEnemies = 0;
    private int currentRangedEnemies = 0;
    private int round = 1;
    
    public GameObject winMenuUI;
    public TextMeshProUGUI healthText;

   
    public void WinGame()
    {
        winMenuUI.SetActive(true);
        Time.timeScale = 0f;

        var mainUiController = FindObjectOfType<MainController>();
        if (mainUiController != null)
        {
            int currentScore = mainUiController.GetCurrentScore();
            string gameDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            RankingEntry newEntry = new RankingEntry(gameDateTime, currentScore);
            RankingManager.SaveRanking(newEntry);
        }
    }


    void Start()
    {
        StartCoroutine(GameRound());
    }
    IEnumerator GameRound()
    {
        if (round == 1)
        {
            for (int i = 0; i < 10; i++) SpawnEnemy(true);
        }

        yield return new WaitForSeconds(gameTimer);
        Debug.Log("Round 1 ends");

        if (round == 1)
        {
            StartRoundTwo();
            Debug.Log("Round 2 starts");
            yield return new WaitForSeconds(gameTimer);
            Debug.Log("Round 2 ends");
        }

        CheckEndGameCondition();
    }
    void CheckEndGameCondition()
    {
        DamageableCharacter player = FindObjectOfType<DamageableCharacter>();
        if (player != null && player.Health > 0)
        {
            WinGame();
        }
        else
        {
            Debug.Log("Game Over. Player died.");
        }
    }

    void StartRoundTwo()
    {
        ClearEnemies(); 
        

        ObstacleSpawner obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        if (obstacleSpawner != null)
        {
            obstacleSpawner.GenerateAllObstacles(); // 使用新的公共方法
        }
        else
        {
            Debug.LogError("ObstacleSpawner instance not found.");
        }
        
        round = 2;
        for (int i = 0; i < 10; i++)
        {
            SpawnEnemy(true);
        }
        for (int i = 0; i < 5; i++)
        {
            SpawnEnemy(false);
        }

        ResetPlayerHealth();
    }
    public static event Action OnRoundTwoStart;
    void ResetPlayerHealth()
    {
        Debug.Log("reset health!!!");
        OnRoundTwoStart?.Invoke();
        
        DamageableCharacter player = FindObjectOfType<DamageableCharacter>();
        if (player != null && player.CompareTag("Player"))
        {
            player.Health = 30;
            player.health = 30;
            HealthDisplay.UpdateHealth(30);

            healthText.text = "Health: " + 30;
        }
    }


    public void SpawnEnemy(bool isMelee)
    {
        if ((isMelee && currentMeleeEnemies >= (round == 1 ? 10 : 15)) || (!isMelee && currentRangedEnemies >= 5))
        {
            return; 
        }

        GameObject prefab = isMelee ? meleeEnemyPrefab : rangedEnemyPrefab;
        Vector2 spawnPoint;
        bool canSpawn;
        do
        {
            spawnPoint = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            canSpawn = !Physics2D.OverlapCircle(spawnPoint, 0.5f, obstacleLayer);
        } while (!canSpawn);

        GameObject enemy = Instantiate(prefab, spawnPoint, Quaternion.identity);
        enemies.Add(enemy);

        if (isMelee)
        {
            currentMeleeEnemies++;
            enemy.GetComponent<DamageableCharacter>().OnDeath += () => StartCoroutine(RespawnEnemy(true));
        }
        else
        {
            currentRangedEnemies++;
            enemy.GetComponent<DamageableCharacter>().OnDeath += () => StartCoroutine(RespawnEnemy(false));
        }
    }


    IEnumerator RespawnEnemy(bool isMelee)
    {
        yield return new WaitForSeconds(isMelee ? 2f : 4f);

        if (round == 2)
        {
            if (isMelee && currentMeleeEnemies < 15)
            {
                SpawnEnemy(true);
            }
            else if (!isMelee && currentRangedEnemies < 5)
            {
                SpawnEnemy(false);
            }
        }
    }
    


    void ClearEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
        currentMeleeEnemies = 0;
        currentRangedEnemies = 0;
    }


    IEnumerator CheckWinCondition()
    {
        yield return new WaitForSeconds(gameTimer);

        if (round == 1)
        {
            StartRoundTwo();
            yield return new WaitForSeconds(gameTimer);
        }

        DamageableCharacter player = FindObjectOfType<DamageableCharacter>();
        if (player != null && player.Health > 0)
        {
            WinGame();
        }
        else
        {
            Debug.Log("Game Over");
        }
    }


}