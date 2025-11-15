using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;

    [Header("Player Reference")]
    [SerializeField] private Transform playerOrientation;

    private Camera playerCam;

    [Header("Camera Parameters")]
    [SerializeField] private float mouseXSensitivity = 10f;
    [SerializeField] private float mouseYSensitivity = 10f;
    [SerializeField] private float sensMultiplier = 0.2f;
    [SerializeField] private float maxRotation = 80f;
    [SerializeField] private float minRotation = -80f;

    private float verticalRotation;
    private float horizontalRotation;

    void Start()
    {
        inputHandler = PlayerInputHandler.Instance;
        playerCam = GetComponent<Camera>();
        HideCursor();
    }

    void Update()
    {
        HandleRotation();
        UpdateCameraPosition();
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetCameraFOV(float fov)
    {
        playerCam.fieldOfView = fov;
    }

    private void HandleRotation()
    {
        horizontalRotation += inputHandler.LookInputVector.x * sensMultiplier * mouseXSensitivity;
        verticalRotation -= inputHandler.LookInputVector.y * sensMultiplier * mouseYSensitivity;

        // Rotate Player on Y axis
        playerOrientation.rotation = Quaternion.Euler(0, horizontalRotation, 0);

        // Rotate Camera on X and Y axis
        verticalRotation = Mathf.Clamp(verticalRotation, minRotation, maxRotation);
        playerCam.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    private void UpdateCameraPosition()
    {
        transform.position = playerOrientation.position;
    }
}
