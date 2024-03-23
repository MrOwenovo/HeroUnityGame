using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    private float startTime;
    private int score = 0;
    
    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        UpdateTimeText();
        UpdateScoreText();

    }
    public int GetCurrentScore()
    {
        return score;
    }
    private void UpdateTimeText()
    {
        float timeSinceStart = Time.time - startTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeSinceStart);
        timeText.text = "Time: " + timeSpan.ToString(@"mm\:ss");
    }
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    public void SaveRank()
    {
        string gameDateTimeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        RankingEntry newEntry = new RankingEntry(gameDateTimeStr, score);
        RankingManager.SaveRanking(newEntry);
    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    

}
