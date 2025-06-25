using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManger : MonoBehaviour
{
    public static ScoreManger Instance;

    private int score = 0;

    [SerializeField] public TMP_Text score_Text;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }
    private void UpdateScoreUI()
    {
        if (score_Text != null)
        {
            score_Text.text = "Score: " + score;
        }
    }
    public int GetScore()
    {
        return score;
    }
}
