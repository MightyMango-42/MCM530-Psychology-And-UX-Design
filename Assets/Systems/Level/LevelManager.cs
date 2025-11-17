using UnityEngine;

public class LevelManager : MonoBehaviour
{
    PlayerController player;
    private int score;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnLevelEnd()
    {
        score = CalculateLevelScore();
    }

    private int CalculateLevelScore()
    {
        int calculatedScore = player.score;
        float levelTime = player.time;

        return calculatedScore;
    }
}
