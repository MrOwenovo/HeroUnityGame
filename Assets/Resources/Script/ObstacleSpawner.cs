using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public int gridSize = 23; // 网格大小
    public int nonDesObstacleCount = 15; // 不可破坏障碍物数量
    public int wreckableObstacleCount = 15; // 可破坏障碍物数量
    public float edgeBuffer = 1; // 边缘缓冲区，确保障碍物不会生成在边界上

    private void Start()
    {
        SpawnObstacles(GameAssets.Instance.NonDesObstacle, nonDesObstacleCount);
        SpawnObstacles(GameAssets.Instance.WreckableObstacle, wreckableObstacleCount);
    }

    public void GenerateAllObstacles()
    {
        SpawnObstacles(GameAssets.Instance.NonDesObstacle, nonDesObstacleCount);
        SpawnObstacles(GameAssets.Instance.WreckableObstacle, wreckableObstacleCount);
    }

    private void SpawnObstacles(Transform obstaclePrefab, int count)
    {
        HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
        Vector3 centerPosition = transform.position; // 获取 ObstacleCenter 的位置

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = Vector3.zero;
            do
            {
                // 考虑边缘缓冲区，调整障碍物的生成位置范围
                float x = Random.Range(-gridSize / 2 + edgeBuffer, gridSize / 2 - edgeBuffer);
                float y = Random.Range(-gridSize / 2 + edgeBuffer, gridSize / 2 - edgeBuffer);
                // 将障碍物的相对位置转换为世界坐标
                spawnPosition = new Vector3(x + centerPosition.x, y + centerPosition.y, 0);
            }
            while (occupiedPositions.Contains(spawnPosition));

            occupiedPositions.Add(spawnPosition);
            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity, transform);
        }
    }
}