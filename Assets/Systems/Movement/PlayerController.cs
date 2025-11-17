using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;

    private Rigidbody rb;

    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private CameraController cameraController;
    public PlayerHUDManager playerHUDManager;

    [Header("Info")]
    public int score = 0;
    public float playTime = 0f;
    public int deathCount = 0;
    private float playerHeight;

    [Header("Movement Parameters")]
    [SerializeField] private float groundSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    [SerializeField] private float airSpeedMultiplier;
    [SerializeField] private float generalForceMultiplier;

    private float travelSpeed;
    private float currentSpeed;
    Vector3 moveDirection;
    Vector3 lastPosition;

    private bool allowMovement = true;
    private bool isGrounded = true;

    [Header("Jump Parameters")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float collisionSphereSize;
    [SerializeField] private float maxCollisionDistance;
    [SerializeField] private float jumpForce;

    [Header("Wall Run Parameters")]
    [SerializeField] LayerMask wallrunLayer;
    [SerializeField] private float wallRunForce;
    [SerializeField] private float wallCheckDistance;

    [Header("Wall Run Jump Parameters")]
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool canRunLeft;
    private bool canRunRight;
    private bool isWallRunning;

    [Header("Slide Parameters")]
    [SerializeField] private float slideForce;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideHeight;
    private bool isSliding;

    [Header("Dash Parameters")]
    [SerializeField] private float dashForce = 5f;
    private bool canDash;
    private bool hasDash;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        inputHandler = PlayerInputHandler.Instance;

        rb.freezeRotation = true;

        inputHandler.jumpAction.performed += Jump;
        inputHandler.slideAction.performed += Slide;
        inputHandler.slideAction.canceled += ExitSlide;

        playerHeight = transform.localScale.y;
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        if (!allowMovement) return;

        CalculateSpeed();
        PassInCameraData();
        HandlePlayerInput();
        AddPlayerDrag();
        LimitSpeed(travelSpeed);
        UpdateHUD();
    }

    private void FixedUpdate()
    {
        isGrounded = GroundCheck();
        HandleMovement();
        HandleWallRunning();
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

    public void Kill()
    {
        deathCount++;
    }

    private void HandlePlayerInput()
    {
        Vector2 moveInputVector = inputHandler.MoveInputVector;
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
            travelSpeed = groundSpeed;
            rb.AddForce(moveDirection * travelSpeed, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            travelSpeed = groundSpeed / 2;
            rb.AddForce(moveDirection * travelSpeed * airSpeedMultiplier, ForceMode.Force);
        }
    }

    private void LimitSpeed(float maxSpeed)
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Check if the magnitude of the player's velocity is greater than the max speed
        // If it is, normalise it and apply it to the rb.linearVelocity's X and Z

        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * travelSpeed;
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
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
    private bool CheckCanWallRun()
    {
        if (isSliding) return false;

        canRunLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wallrunLayer);
        canRunRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wallrunLayer);

        if (canRunLeft || canRunRight) return true;
        return false;
    }

    private void EnterWallRun()
    {
        isWallRunning = true;
        rb.useGravity = false;
    }

    private void ExitWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;
    }
    private void HandleWallRunning()
    {
        if (CheckCanWallRun())
        {
            EnterWallRun();

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            Vector3 wallNormal = canRunRight ? -rightWallHit.normal : leftWallHit.normal;
            Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

            rb.AddForce(wallForward * wallRunForce * Time.fixedDeltaTime * generalForceMultiplier, ForceMode.Force);

            if (inputHandler.JumpTriggered) WallJump();
        }
        else if (!CheckCanWallRun()) ExitWallRun();
    }

    private void WallJump()
    {
        // Get the wall normal, obtain the jump direction force by multiplying each transform direction
        // Lets the player jump off of a wall forwards at a diagonal angle away from the wall normal
        Vector3 wallNormal = canRunRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 jumpDirection = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // Apply the force, dont need to reset yVelocity as it is already done in HandleWallRunning()
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(jumpDirection * Time.fixedDeltaTime * (generalForceMultiplier / 4), ForceMode.Impulse);
    }

    private void Slide(InputAction.CallbackContext context)
    {
        if (!(moveDirection.x > 0 || moveDirection.z > 0)) return;
        if (isWallRunning) return;

        isSliding = true;
        allowMovement = false;

        transform.localScale = new Vector3 (transform.localScale.x, slideHeight, transform.localScale.z);
        rb.AddForce(moveDirection * slideForce * Time.fixedDeltaTime, ForceMode.Force);
    }

    private void ExitSlide(InputAction.CallbackContext context)
    {
        isSliding = false;
        transform.localScale = new Vector3(transform.localScale.x, playerHeight, transform.localScale.z);
        allowMovement = true;
    }

    private void PassInCameraData()
    {
        cameraController.SetSpeedEffect(currentSpeed, 10);
    }

    private void CalculateSpeed()
    {
        float distance = Vector3.Distance(lastPosition, transform.position);
        currentSpeed = distance / Time.deltaTime;
        lastPosition = transform.position;
    }

    private void UpdateHUD()
    {
        playerHUDManager.UpdateTimer(playTime);
        playerHUDManager.SetPlayerSpeed(currentSpeed);
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
