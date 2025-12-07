using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;

    [Header("Player Reference")]
    [SerializeField] private Transform playerOrientation;

    private Camera playerCam;

    [Header("Camera Parameters")]
    [SerializeField] private float mouseSensX;
    [SerializeField] private float mouseSensY;
    [SerializeField] private float gamepadSensX;
    [SerializeField] private float gamepadSensY;
    [SerializeField] private float sensMultiplier;
    [SerializeField] private float maxRotation;
    [SerializeField] private float minRotation;
    [SerializeField] private float yOffset;

    private float verticalRotation;
    private float horizontalRotation;
    private float zRotation;

    [Header("FOV Settings")]
    [SerializeField] private float baseFOV;
    [SerializeField] private float maxFOV;
    private float playerSpeed;
    private float playerMinSpeed;

    [Header("Tilt Parameters")]
    [SerializeField] public float wallRunTiltAmount;
    [SerializeField] private float rotationSpeed;
    private float zAngle;

    [Header("VFX")]
    [SerializeField] GameObject speedLinesObject;
    [SerializeField] private float speedLineDeactivationTime;
    private float speedLineTimer;

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
        UpdateEffects();
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

    public void Tilt(float angle, bool toRight)
    {
        if (toRight) zRotation = angle;
        else zRotation = -angle;
    }

    private void HandleRotation()
    {
        float sensX;
        float sensY;

        if (inputHandler.playerInputComponent.currentControlScheme == "Gamepad")
        {
            sensX = gamepadSensX;
            sensY = gamepadSensY;
        }
        else
        {
            sensX = mouseSensX;
            sensY = mouseSensY;
        }
            

        horizontalRotation += inputHandler.LookInputVector.x * sensMultiplier * sensX * Time.deltaTime;
        verticalRotation -= inputHandler.LookInputVector.y * sensMultiplier * sensY * Time.deltaTime;

        // Rotate Player on Y axis
        playerOrientation.rotation = Quaternion.Euler(0, horizontalRotation, 0);

        // Rotate Camera on X and Y axis
        verticalRotation = Mathf.Clamp(verticalRotation, minRotation, maxRotation);

        // Lerp Z angle for smoother transition
        zAngle = Mathf.Lerp(zAngle, zRotation, Time.deltaTime * rotationSpeed);

        playerCam.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, zAngle);
    }

    private void UpdateCameraPosition()
    {
        transform.position = new Vector3(playerOrientation.position.x, playerOrientation.position.y + yOffset, playerOrientation.position.z);
    }

    private void UpdateEffects()
    {
        speedLineTimer -= Time.deltaTime;

        if (playerSpeed < playerMinSpeed)
        {
            playerCam.fieldOfView = baseFOV;

            if (speedLineTimer <= 0)
            {
                DeactivateSpeedLines();
            }
            return;
        }

        float fovToAdd = (playerSpeed - playerMinSpeed) * 1.2f;
        if (baseFOV + fovToAdd > maxFOV) return;

        playerCam.fieldOfView = baseFOV + fovToAdd;
        ActivateSpeedLines();
    }

    public void SetSpeedEffect(float speed, float minSpeed)
    {
        playerSpeed = speed;
        playerMinSpeed = minSpeed;
    }

    public void ActivateSpeedLines()
    {
        speedLineTimer = speedLineDeactivationTime;
        speedLinesObject.SetActive(true);
    }

    public void DeactivateSpeedLines()
    { 
        speedLinesObject.SetActive(false);
    }
}
