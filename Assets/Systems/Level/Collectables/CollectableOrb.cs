using UnityEngine;

public class CollectableOrb : MonoBehaviour
{
    [SerializeField] private int scoreValue = 500;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            LevelManager levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

            levelManager.foundCollectables++;

            controller.score += scoreValue;
            controller.playerHUDManager.StartCoroutine(controller.playerHUDManager.OpenPopupPanel("Collectable found!")); ;

            Destroy(gameObject);
        }
    }
}
