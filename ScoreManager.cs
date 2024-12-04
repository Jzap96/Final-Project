using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Link to UI Text element
    private int score = 0;           // Initialize score

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score += points; // Increment score
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score; // Update UI text
    }
}