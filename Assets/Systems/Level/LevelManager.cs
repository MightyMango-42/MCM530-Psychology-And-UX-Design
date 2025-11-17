using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    PlayerController player;
    public int TotalScore { get; private set; }

    [SerializeField] private Vector3 spawnPoint = new Vector3(0, 1.1f, 0);

    [Header("Level Contents")]
    [SerializeField] private List<CollectableOrb> collectables;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.transform.position = spawnPoint;
    }

    public void RespawnPlayer()
    {
        player.Kill();
        player.transform.position = spawnPoint;
    }

    public void EndLevel()
    {
        TotalScore = CalculateLevelScore();
        Debug.Log("Reached End of Level");
    }

    private int CalculateLevelScore()
    {
        int calculatedScore = player.score;
        float levelTime = player.playTime;

        // Do calculation

        return calculatedScore;
    }
}
