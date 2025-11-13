using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public PlayerInputActions inputActions;

    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction lookAction;

    private Vector3 direction;
    private Vector3 velocity;
    private float verticalRotation;
    private float horizontalRotation;

    [Header("References")]
    [SerializeField] Camera playerCam;

    [Header("Movement Parameters")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private float sprintSpeed;
    private float moveSpeed;

    [Header("Jump Parameters")]
    [SerializeField] private float gravityStrength = 9.81f;
    [SerializeField] private float jumpForce;

    [Header("Camera Parameters")]
    [SerializeField] private float mouseXSensitivity = 10f;
    [SerializeField] private float mouseYSensitivity = 10f;
    [SerializeField] private float sensMultiplier = 0.2f;

    [SerializeField] private float maxRotation = 80f;
    [SerializeField] private float minRotation = -80f;

    public float currentFOV;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction = inputActions.Player.Move;
        sprintAction = inputActions.Player.Sprint;
        jumpAction = inputActions.Player.Jump;
        lookAction = inputActions.Player.Look;

        moveAction.Enable();
        sprintAction.Enable();
        jumpAction.Enable();
        lookAction.Enable();

        sprintAction.performed += Sprint;
    }
    private void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable();
        jumpAction.Disable();
        lookAction.Disable();
    }

    private void Start()
    {
        HideCursor();
    }

    private void Update()
    {
        ApplyGravity();
        HandleInputs();
        HandleJumping();
        HandleRotation();
        ApplyMovement();
    }   

    private void HandleInputs()
    {
        moveSpeed = (sprintAction.inProgress ? sprintSpeed : baseSpeed);

        Vector2 moveInputVector = moveAction.ReadValue<Vector2>();

        direction = transform.forward * moveInputVector.y + transform.right * moveInputVector.x;
        direction.Normalize();

        velocity.x = direction.x * moveSpeed;
        velocity.z = direction.z * moveSpeed;
    }

    private void ApplyMovement()
    {
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleJumping()
    {
        // Apply a small force if the character controller is grounded to prevent isGrounded true/false inaccuraccies
        if (characterController.isGrounded)
        {
            velocity.y = 0;
            velocity.y -= 1f;

            if (jumpAction.WasPressedThisFrame())
            {
                velocity.y = jumpForce;
            }
        }
    }

    private void ApplyGravity()
    {
        velocity.y -= gravityStrength * Time.deltaTime;
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        moveSpeed = sprintSpeed;
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

    private void HandleRotation()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        horizontalRotation += lookInput.x * sensMultiplier * mouseXSensitivity;
        verticalRotation -= lookInput.y * sensMultiplier * mouseYSensitivity;

        // Rotate Player on Y axis
        transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);

        // Rotate Camera on X and Y axis
        verticalRotation = Mathf.Clamp(verticalRotation, minRotation, maxRotation);
        playerCam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
