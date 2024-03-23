using System;
using TMPro;
using UnityEngine;

public class RankingDisplay : MonoBehaviour
{
    public GameObject rankingEntryPrefab;
    public Transform contentTransform;

    void Start()
    {
        // RankingManager.InitializeTestData();
        DisplayRanking();
        RankingManager.PrintCurrentRankingData();
        
    }

    void DisplayRanking()
    {
        var rankingList = RankingManager.LoadRanking();
        rankingList.rankings.Sort((x, y) => y.score.CompareTo(x.score));

        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < rankingList.rankings.Count; i++)
        {
            var entry = rankingList.rankings[i];
            GameObject entryObj = Instantiate(rankingEntryPrefab, contentTransform);
            entryObj.GetComponent<TextMeshProUGUI>().text = $"Rank {i + 1}: Score {entry.score}, Date: {entry.gameDateTime}";
        }
    }

    private string FormatGameDateTime(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

   
}