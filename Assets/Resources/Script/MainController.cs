using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainController : MonoBehaviour
{
    public TextMeshProUGUI timeText; // UI Text元素的引用
    public TextMeshProUGUI scoreText; // 得分显示
    private float startTime;
    private int score = 0;
    
    void Start()
    {
        startTime = Time.time; // 记录游戏开始的时间
    }

    void Update()
    {
        UpdateTimeText();
        UpdateScoreText();

    }
    private void UpdateTimeText()
    {
        float timeSinceStart = Time.time - startTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeSinceStart);
        timeText.text = "Time: " + timeSpan.ToString(@"mm\:ss"); // 格式化时间显示
    }
    public void AddScore(int amount)
    {
        score += amount; // 增加得分
        UpdateScoreText(); // 更新得分显示
    }

    public void SaveRank()
    {
        string gameDateTimeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // 获取当前日期和时间并格式化为字符串
        RankingEntry newEntry = new RankingEntry(gameDateTimeStr, score);
        RankingManager.SaveRanking(newEntry);
    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString(); // 更新得分显示
    }
    

}
