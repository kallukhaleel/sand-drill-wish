using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoardManager : MonoBehaviour
{
    public TextMeshProUGUI HighScore;
    public TextMeshProUGUI BestTime;
    public void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        string formattedTime = bestTime == float.MaxValue
            ? "N/A" 
            : $"{Mathf.Floor(bestTime / 60): 00}: {Mathf.FloorToInt(bestTime % 60): 00}";

        HighScore.text = "HIGH SCORE:" + highScore;
        BestTime.text = "BEST TIME:" + formattedTime;
    }
}