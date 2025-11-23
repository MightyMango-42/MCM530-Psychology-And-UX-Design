using Unity.VisualScripting;
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
    [SerializeField] private float xSensitivity;
    [SerializeField] private float ySensitivity;
    [SerializeField] private float sensMultiplier;
    [SerializeField] private float maxRotation;
    [SerializeField] private float minRotation;
    [SerializeField] private float yOffset;

    private float verticalRotation;
    private float horizontalRotation;

    [Header("FOV Settings")]
    [SerializeField] private float baseFOV;
    [SerializeField] private float maxFOV;
    private float playerSpeed;
    private float playerMinSpeed;

    public bool allowMovement = true;

    void Start()
    {
        inputHandler = PlayerInputHandler.Instance;
        playerCam = GetComponent<Camera>();
        playerCam.fieldOfView = baseFOV;
        HideCursor();
    }

    void Update()
    {
        if (!allowMovement) return;
        HandleRotation();
        UpdateCameraPosition();
        UpdateFOV();
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
        horizontalRotation += inputHandler.LookInputVector.x * sensMultiplier * xSensitivity * Time.deltaTime;
        verticalRotation -= inputHandler.LookInputVector.y * sensMultiplier * ySensitivity * Time.deltaTime;

        // Rotate Player on Y axis
        playerOrientation.rotation = Quaternion.Euler(0, horizontalRotation, 0);

        // Rotate Camera on X and Y axis
        verticalRotation = Mathf.Clamp(verticalRotation, minRotation, maxRotation);
        playerCam.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    private void UpdateCameraPosition()
    {
        transform.position = new Vector3(playerOrientation.position.x, playerOrientation.position.y + yOffset, playerOrientation.position.z);
    }

    private void UpdateFOV()
    {
        if (playerSpeed < playerMinSpeed)
        {
            playerCam.fieldOfView = baseFOV;
            return;
        }

        float fovToAdd = (playerSpeed - playerMinSpeed) * 1.2f;
        if (baseFOV + fovToAdd > maxFOV) return;

        playerCam.fieldOfView = baseFOV + fovToAdd;
    }

    public void SetSpeedEffect(float speed, float minSpeed)
    {
        playerSpeed = speed;
        playerMinSpeed = minSpeed;
    }
}
