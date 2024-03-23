using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RankingEntry
{
    public string gameDateTime;
    public int score;

    public RankingEntry(string gameDateTime, int score)
    {
        this.gameDateTime = gameDateTime;
        this.score = score;
    }
}

[Serializable]
public class RankingList
{
    public List<RankingEntry> rankings = new List<RankingEntry>();
}