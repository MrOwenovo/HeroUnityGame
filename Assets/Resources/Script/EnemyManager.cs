using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject meleeEnemyPrefab; // 近战敌人预制体
    public int maxEnemies = 10; // 最大敌人数量
    private List<GameObject> enemies = new List<GameObject>(); // 存储当前场景中的敌人
    public float spawnRadius = 11.5f; // 生成半径，确保和障碍物生成的范围一致
    public LayerMask obstacleLayer; // 障碍物层，用于检测生成位置是否有效
    private float gameTimer = 30f; // 游戏时间，30秒后胜利

    void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }

        StartCoroutine(CheckWinCondition());
    }

    void Update()
    {
        // 重新生成逻辑在敌人死亡时处理
    }

    public void SpawnEnemy()
    {
        Vector2 spawnPoint;
        bool canSpawn = false;
        do
        {
            spawnPoint = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            canSpawn = !Physics2D.OverlapCircle(spawnPoint, 0.5f, obstacleLayer);
        } while (!canSpawn);

        GameObject enemy = Instantiate(meleeEnemyPrefab, spawnPoint, Quaternion.identity);
        enemies.Add(enemy);
        enemy.GetComponent<DamageableCharacter>().OnDeath += () => StartCoroutine(RespawnEnemy());
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(2f); // 等待2秒后重生

        if (enemies.Count < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    IEnumerator CheckWinCondition()
    {
        yield return new WaitForSeconds(gameTimer); // 等待游戏时间结束
        Debug.Log("Win!"); // 这里替换为游戏胜利逻辑
    }
}