using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EndZone : MonoBehaviour
{
    LevelManager levelManager;
    [SerializeField] private Vector3 size;
    [SerializeField] private GameObject rotatingObject;
    [SerializeField] private float rotationDelay;
    [SerializeField] private string sceneToLoad;

    private void Awake()
    {
        levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            levelManager.EndLevel();
            GameManager.Instance.LoadScene(sceneToLoad);
        }
    }

    // Draw a green wire-box at the end zone's transform
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
