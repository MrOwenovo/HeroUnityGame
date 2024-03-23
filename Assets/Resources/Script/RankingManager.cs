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
        rankingList.rankings.Sort((x, y) => y.score.CompareTo(x.score));

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
        DateTime now = DateTime.Now;
        DateTime testDate1 = now.AddDays(-1);
        DateTime testDate2 = now.AddHours(-2);
        DateTime testDate3 = now.AddDays(-7);

        List<RankingEntry> testEntries = new List<RankingEntry>
        {
            new RankingEntry(testDate1.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), 100),
            new RankingEntry(testDate2.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), 150),
            new RankingEntry(testDate3.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), 80)
        };

        RankingList rankingList = new RankingList {rankings = testEntries};
    
        string json = JsonUtility.ToJson(rankingList);
        PlayerPrefs.SetString(rankingFilePath, json);
        PlayerPrefs.Save();
    }


}