using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Checkpoint : MonoBehaviour
{
    LevelManager levelManager;

    public int ID = -1;
    [SerializeField] private Vector3 size;
    [SerializeField] private Vector3 respawnPosition;

    private void Awake()
    {
        levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
    }

    private void Start()
    {
        if (respawnPosition == Vector3.zero)
        {
            respawnPosition = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (ID < 0) 
            {
                Debug.LogError("Checkpoint ID not assigned, please add it to the checkpoint list within the level manager");
                return;
            } 
            
            levelManager.SetNewCheckpoint(respawnPosition, ID);
        }
    }

    // Draw a blue wire-box at the checkpoint's transform
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
