using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ObjectAppearanceZone : MonoBehaviour
{
    [SerializeField] private Vector3 size;
    [SerializeField] private GameObject obj;

    [SerializeField] private bool disappearOnExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            obj.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!disappearOnExit) return;

        if (other.gameObject.CompareTag("Player"))
        {
            obj.SetActive(false);
        }
    }

    // Draw a green wire-box at the end zone's transform
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
