using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public int gridSize = 23;
    public int nonDesObstacleCount = 15;
    public int wreckableObstacleCount = 15;
    public float edgeBuffer = 1;

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
        Vector3 centerPosition = transform.position;

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = Vector3.zero;
            do
            {
                float x = Random.Range(-gridSize / 2 + edgeBuffer, gridSize / 2 - edgeBuffer);
                float y = Random.Range(-gridSize / 2 + edgeBuffer, gridSize / 2 - edgeBuffer);
                spawnPosition = new Vector3(x + centerPosition.x, y + centerPosition.y, 0);
            }
            while (occupiedPositions.Contains(spawnPosition));

            occupiedPositions.Add(spawnPosition);
            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity, transform);
        }
    }
}