using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    PlayerController player;
    public int TotalScore { get; private set; }

    [SerializeField] private Vector3 spawnPoint = new Vector3(0, 1.1f, 0);
    private Vector3 respawnPoint;

    [Header("Level Contents")]
    [SerializeField] private List<Checkpoint> checkpoints;
    [SerializeField] private List<CollectableOrb> collectables;

    public int currentCheckpoint = -1;
    public int foundCollectables = 0;
    public int CollectableCount { get; private set; }

    private void Start()
    {
        respawnPoint = spawnPoint;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.transform.position = spawnPoint;
        player.UnFreeze();

        CollectableCount = collectables.Count;

        // Auto-assign checkpoint ID's
        for (int i = 0; i < checkpoints.Count; i++)
        {
            checkpoints[i].ID = i;
        }
    }

    public void RespawnPlayer()
    {
        player.Kill();
        player.transform.position = respawnPoint;
    }

    public void EndLevel()
    {
        TotalScore = CalculateLevelScore();
        string collectableStats = $"{foundCollectables}/{CollectableCount}";

        player.playerHUDManager.OpenLevelCompletionPanel(TotalScore, collectableStats);

        player.UnregisterCallbacks();
        player.Freeze();
        player.KillMomentum();
    }

    public void SetNewCheckpoint(Vector3 newSpawnPosition, int newCheckPoint)
    {
        if (newCheckPoint <= currentCheckpoint) return;

        currentCheckpoint = newCheckPoint;
        respawnPoint = newSpawnPosition;
    }

    private int CalculateLevelScore()
    {
        int calculatedScore = player.score;
        float levelTime = player.playTime;

        // Do calculation

        calculatedScore += GameManager.Instance.maxLevelScore;

        return calculatedScore;
    }
}
