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

    [SerializeField] private float gravity = 9.81f;

    private Vector3 direction;
    private Vector3 velocity;

    [SerializeField] private float baseSpeed;
    [SerializeField] private float sprintSpeed;
    private float moveSpeed;

    [SerializeField] private float jumpForce;
    [SerializeField] private float maxJumpDuration;
    private float jumpTime;
    private bool isJumping;

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

        moveAction.Enable();
        sprintAction.Enable();
        jumpAction.Enable();

        sprintAction.performed += Sprint;
    }
    private void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable();
        jumpAction.Disable();
    }

    private void Start()
    {
        moveSpeed = baseSpeed;
    }

    private void Update()
    {
        ApplyGravity();
        UpdateMovement();
        HandleJumping();
        ApplyMovement();
    }

    private void UpdateMovement()
    {
        Vector2 inputVelocity = moveAction.ReadValue<Vector2>();
        
        if (sprintAction.WasReleasedThisFrame()) moveSpeed = baseSpeed;
        direction = new Vector3(inputVelocity.x * moveSpeed, velocity.y, inputVelocity.y * moveSpeed);
    }

    private void ApplyMovement()
    {
        characterController.Move(direction * Time.deltaTime);
    }

    private void HandleJumping()
    {
        if (characterController.isGrounded && jumpAction.WasPressedThisFrame())
        {
            velocity.y = jumpForce;
        }

        if (jumpAction.WasReleasedThisFrame() && velocity.y > 0f)
        {
            velocity = new Vector3(velocity.x, velocity.y * 0.5f, velocity.z);
        }
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            velocity.y -= gravity;
        }
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        moveSpeed = sprintSpeed;
    }
}
