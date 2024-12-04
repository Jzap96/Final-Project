using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreZoneTrigger : MonoBehaviour
{
    public ScoreManager scoreManager; // Reference to the ScoreManager
    public int pointsPerEnemy = 10;   // Points to add for passing this zone
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player passing through
        {
            scoreManager.AddScore(pointsPerEnemy); // Add points for passing
        }
    }
}