using System;
using TMPro;
using UnityEngine;

public class RankingDisplay : MonoBehaviour
{
    public GameObject rankingEntryPrefab; // 排名项预制体
    public Transform contentTransform; // Scroll View 下的 Content 对象的 Transform

    void Start()
    {
        // RankingManager.InitializeTestData();
        DisplayRanking();
        RankingManager.PrintCurrentRankingData();
        
    }

    void DisplayRanking()
    {
        var rankingList = RankingManager.LoadRanking();
        rankingList.rankings.Sort((x, y) => y.score.CompareTo(x.score)); // 按得分从高到低排序

        // 在显示之前清除之前的排名项，以防止重复
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < rankingList.rankings.Count; i++)
        {
            var entry = rankingList.rankings[i];
            GameObject entryObj = Instantiate(rankingEntryPrefab, contentTransform);
            // 动态计算排名，并展示得分和游戏结束的日期和时间
            // 直接使用 entry.gameDateTime，因为它已经是一个格式化后的字符串
            entryObj.GetComponent<TextMeshProUGUI>().text = $"Rank {i + 1}: Score {entry.score}, Date: {entry.gameDateTime}";
        }
    }

    private string FormatGameDateTime(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

   
}