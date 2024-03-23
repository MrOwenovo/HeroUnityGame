using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public GameObject heartPrefab;
    public int maxHealth = 30;
    private static List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            hearts.Add(heart);

            int column = i / 10;
            int row = i % 10;
            heart.transform.localPosition = new Vector3(column * 10, -row * 10, 0);
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