using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject meleeEnemyPrefab; // 近战敌人预制体
    public int maxEnemies = 10; // 最大敌人数量
    private List<GameObject> enemies = new List<GameObject>(); // 存储当前场景中的敌人
    public float spawnRadius = 10.5f; // 生成半径，确保和障碍物生成的范围一致
    public LayerMask obstacleLayer; // 障碍物层，用于检测生成位置是否有效
    private float gameTimer = 30f; // 游戏时间，30秒后胜利
    
    public GameObject rangedEnemyPrefab; // 远程敌人预制体
    private int currentMeleeEnemies = 0;
    private int currentRangedEnemies = 0;
    private int round = 1; // 当前游戏轮次
    
    public GameObject winMenuUI;
   
    public void WinGame()
    {
        winMenuUI.SetActive(true);
        Time.timeScale = 0f; // 暂停游戏
    }

    void Start()
    {
        // 第一轮开始时生成15个近战敌人
        for (int i = 0; i < 15; i++)
        {
            SpawnEnemy(true); // 传递 true 表示生成近战敌人
        }

        StartCoroutine(CheckWinCondition());
    }




    public void SpawnEnemy(bool isMelee)
    {
        GameObject prefab = isMelee ? meleeEnemyPrefab : rangedEnemyPrefab;
        Vector2 spawnPoint;
        bool canSpawn = false;
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
        yield return new WaitForSeconds(isMelee ? 2f : 4f); // 近战敌人2秒重生，远程敌人4秒重生

        if ((isMelee && currentMeleeEnemies < 10) || (!isMelee && currentRangedEnemies < 5))
        {
            SpawnEnemy(isMelee);
        }
    }

    void StartRoundTwo()
    {
        ClearEnemies(); // 清除所有敌人

        // 获取 ObstacleSpawner 实例并调用其公共方法来重新生成障碍物
        ObstacleSpawner obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        if (obstacleSpawner != null)
        {
            obstacleSpawner.GenerateAllObstacles(); // 使用新的公共方法
        }
        else
        {
            Debug.LogError("ObstacleSpawner instance not found.");
        }

        // 生成第二轮的敌人
        round = 2; // 更新轮次标记
        for (int i = 0; i < 10; i++) // 生成10个近战敌人
        {
            SpawnEnemy(true);
        }
        for (int i = 0; i < 5; i++) // 生成5个远程敌人
        {
            SpawnEnemy(false);
        }
    }


    void ClearEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Orc"))
        {
            Destroy(enemy);
        }
        // 重置敌人计数
        currentMeleeEnemies = 0;
        currentRangedEnemies = 0;
    }

    IEnumerator CheckWinCondition()
    {
        yield return new WaitForSeconds(gameTimer); // 等待第一轮游戏时间结束

        if (round == 1)
        {
            StartRoundTwo();
        }
        else
        {
            Debug.Log("Win!"); // 第二轮结束，游戏胜利
            WinGame();
        }
    }

}