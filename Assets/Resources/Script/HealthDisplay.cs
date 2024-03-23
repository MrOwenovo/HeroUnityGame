using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public GameObject heartPrefab; // 心形图标预制体
    public int maxHealth = 30; // 最大生命值
    private static List<GameObject> hearts = new List<GameObject>(); // 存储心形图标的列表

    void Start()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            hearts.Add(heart);

            // 调整位置以形成3列，每列10个
            int column = i / 10;
            int row = i % 10;
            heart.transform.localPosition = new Vector3(column * 10, -row * 10, 0); // 30是假定的间距
        }
    }

    public static void UpdateHealth(int currentHealth)
    {
        Debug.Log("HealthDisplay:"+currentHealth);
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < currentHealth);
        }
    }

    public void ResetHealth()
    {
        UpdateHealth(maxHealth);
    }
}