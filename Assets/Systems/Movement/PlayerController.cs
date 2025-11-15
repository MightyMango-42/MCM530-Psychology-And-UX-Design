using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInputActions inputActions;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction dashAction;

    [Header("References")]
    [SerializeField] private Transform orientation;

    [Header("Movement Parameters")]
    [SerializeField] private float maxGroundSpeed;
    [SerializeField] private float groundSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    [SerializeField] private float airSpeedMultiplier;

    private float currentSpeed;
    Vector3 moveDirection;

    private bool allowMovement = true;
    private bool isGrounded = true;

    [Header("Jump Parameters")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float collisionSphereSize;
    [SerializeField] private float maxCollisionDistance;
    [SerializeField] private float jumpForce = 5f;

    [Header("Dash Parameters")]
    [SerializeField] private float dashForce = 5f;
    private bool canDash;
    private bool hasDash;

    private void OnEnable()
    {
        moveAction = inputActions.Player.Move;
        jumpAction = inputActions.Player.Jump;
        lookAction = inputActions.Player.Look;
        //dashAction = inputActions.Player.Dash;

        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        //dashAction.Enable();

        jumpAction.performed += Jump;
        //dashAction.performed += Dash;
    }
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        //dashAction.Disable();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
    }

    private void Start()
    {
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (!allowMovement) return;
        
        HandlePlayerInput();
        AddPlayerDrag();
        //HandleDashing();
        HandlePlayerSpeed();
    }

    private void FixedUpdate()
    {
        isGrounded = GroundCheck();
        HandleMovement();
    }

    public void Freeze()
    {
        allowMovement = false;
        rb.useGravity = false;
    }
    public void UnFreeze()
    {
        allowMovement = true;
        rb.useGravity = true;
    }

    private void HandlePlayerInput()
    {
        Vector2 moveInputVector = moveAction.ReadValue<Vector2>();
        moveDirection = orientation.forward * moveInputVector.y + orientation.right * moveInputVector.x;
        moveDirection.Normalize();
    }

    private void AddPlayerDrag()
    {
        if (isGrounded) rb.linearDamping = groundDrag;
        else rb.linearDamping = airDrag;
    }
 
    private void HandleMovement()
    {
        if (isGrounded)
        { 
            currentSpeed = groundSpeed;
            rb.AddForce(moveDirection * currentSpeed, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            currentSpeed = groundSpeed / 2;
            rb.AddForce(moveDirection * currentSpeed * airSpeedMultiplier, ForceMode.Force);
        }
    }

    private void HandlePlayerSpeed()
    {
        if (isGrounded)
        {
            LimitSpeedTo(maxGroundSpeed);
        }
    }

    private void LimitSpeedTo(float maxSpeed)
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Check if the magnitude of the player's velocity is greater than the max speed
        // If it is, normalise it and apply it to the rb.linearVelocity's X and Z

        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    private bool GroundCheck()
    {
        // Cast a sphere to the ground, if it hits the specified ground layer stored within the layermask, it will return true

        if (Physics.SphereCast(transform.position, collisionSphereSize, -transform.up, out RaycastHit hit, maxCollisionDistance, layerMask))
        {
            return true;
        }
        else return false;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            //rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleDashing()
    {
        if (hasDash && !isGrounded)
        {
            canDash = true;
        }
        else if (!hasDash)
        {
            canDash = false;
        }

        if (isGrounded)
        {
            hasDash = true;
            canDash = false;
        }
    }

    private void Dash()
    {
        //rb.AddForce(playerCam.transform.forward * dashForce, ForceMode.Impulse);
        hasDash = false;
    }
}
