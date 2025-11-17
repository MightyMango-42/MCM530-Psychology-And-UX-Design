using UnityEngine;

public class CollectableOrb : MonoBehaviour
{
    [SerializeField] private int scoreValue = 500;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            controller.score += scoreValue;
            controller.playerHUDManager.StartCoroutine(controller.playerHUDManager.OpenPopupPanel("Collectable found!")); ;
            Destroy(this.gameObject);
        }
    }
}
