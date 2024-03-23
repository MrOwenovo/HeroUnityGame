using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class RankingManager
{
    private static string rankingFilePath = "rankingData.json";

    public static void SaveRanking(RankingEntry newEntry)
    {
        var rankingList = LoadRanking();
        rankingList.rankings.Add(newEntry);
        rankingList.rankings.Sort((x, y) => y.score.CompareTo(x.score)); // 从高到低排序

        string json = JsonUtility.ToJson(rankingList);
        PlayerPrefs.SetString(rankingFilePath, json);
        PlayerPrefs.Save();
    }

    public static RankingList LoadRanking()
    {
        string json = PlayerPrefs.GetString(rankingFilePath, "{}");
        RankingList rankingList = JsonUtility.FromJson<RankingList>(json);
        return rankingList;
    }
    public static void PrintCurrentRankingData()
    {
        string json = PlayerPrefs.GetString(rankingFilePath, "No data");
        Debug.Log(json);
    }

    public static void InitializeTestData()
    {
        // 获取当前日期和时间，并创建一些测试日期
        DateTime now = DateTime.Now;
        DateTime testDate1 = now.AddDays(-1); // 假设一个游戏是昨天结束的
        DateTime testDate2 = now.AddHours(-2); // 假设一个游戏是两小时前结束的
        DateTime testDate3 = now.AddDays(-7); // 假设一个游戏是一周前结束的

        // 创建一些测试数据
        List<RankingEntry> testEntries = new List<RankingEntry>
        {
            new RankingEntry(testDate1.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), 100),
            new RankingEntry(testDate2.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), 150),
            new RankingEntry(testDate3.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), 80)
        };

        // 清空当前排名信息并保存测试数据
        RankingList rankingList = new RankingList {rankings = testEntries};
    
        string json = JsonUtility.ToJson(rankingList);
        PlayerPrefs.SetString(rankingFilePath, json);
        PlayerPrefs.Save();
    }


}