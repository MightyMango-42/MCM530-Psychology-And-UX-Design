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
    [SerializeField] private float coyoteTimeGiven = 1f;
    private float coyoteTime;

    private float travelSpeed;
    private float currentSpeed;
    Vector3 moveDirection;
    Vector3 lastPosition;

    private bool allowMovement = true;
    private bool isGrounded = true;

    [Header("Jump Parameters")]
    [SerializeField] private LayerMask jumpLayer;
    [SerializeField] private float groundCollisionSphereSize;
    [SerializeField] private float maxCollisionDistance;
    [SerializeField] private float jumpForce;

    [Header("Wall Run Parameters")]
    [SerializeField] LayerMask wallrunLayer;
    [SerializeField] private float wallRunForce;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float wallRunMaxSpeed;

    [Header("Wall Run Jump Parameters")]
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool canRunLeft;
    private bool canRunRight;
    private bool isWallRunning;

    [Header("Slide Parameters")]
    [SerializeField] private LayerMask crouchCheckLayer;
    [SerializeField] private float crouchCollisionSphereSize;
    [SerializeField] private float uncrouchCheckDistance;
    [SerializeField] private float slideForce;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideHeight;
    private bool isSliding;
    private float slidingSpeed;
    private bool canExitSlide;
    private bool attemptingToExitSlide;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        inputHandler = PlayerInputHandler.Instance;

        rb.freezeRotation = true;
        playerHeight = transform.localScale.y;

        RegisterCallbacks();
    }

    private void Update()
    {
        if (GameManager.Instance.GamePaused) return;

        playTime += Time.deltaTime;
        coyoteTime -= Time.deltaTime;

        if (!allowMovement) return;

        CalculateSpeed();
        PassInCameraData();
        HandlePlayerInput();
        AddPlayerDrag();
        LimitSpeed(travelSpeed);

        canExitSlide = CheckCanExitSlide();
        if (isSliding && attemptingToExitSlide) ExitSlide(new InputAction.CallbackContext());

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
        cameraController.allowMovement = false;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
    }
    public void UnFreeze()
    {
        allowMovement = true;
        cameraController.allowMovement = true;
        rb.useGravity = true;
    }

    public void Kill()
    {
        deathCount++;
    }
    public void KillMomentum()
    {
        rb.linearVelocity = Vector3.zero;
    }

    private void ResetCoyoteTime()
    {
        coyoteTime = coyoteTimeGiven;
    }

    private void HandlePlayerInput()
    {
        if (isSliding) return;
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
            rb.AddForce(moveDirection * travelSpeed * Time.fixedDeltaTime * generalForceMultiplier, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            travelSpeed = groundSpeed;
            rb.AddForce(moveDirection * travelSpeed * airSpeedMultiplier * Time.fixedDeltaTime * generalForceMultiplier, ForceMode.Force);
        }
    }

    private void LimitSpeed(float maxSpeed)
    {
        if (isSliding) maxSpeed = slidingSpeed;

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

        if (Physics.SphereCast(transform.position, groundCollisionSphereSize, -transform.up, out RaycastHit hit, maxCollisionDistance, jumpLayer))
        {
            ResetCoyoteTime();
            return true;
        }
        else return false;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (coyoteTime > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce * Time.fixedDeltaTime * (generalForceMultiplier / 4), ForceMode.Impulse);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.jump);
        }
    }
    private bool CheckCanWallRun()
    {
        if (isSliding || isGrounded) return false;

        canRunLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wallrunLayer);
        canRunRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wallrunLayer);

        if (canRunLeft || canRunRight) return true;
        return false;
    }

    private void EnterWallRun()
    {
        ResetCoyoteTime();
        isWallRunning = true;
        rb.useGravity = false;
    }

    private void ExitWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;
        cameraController.Tilt(0, true);
    }
    private void HandleWallRunning()
    {
        if (inputHandler.JumpTriggered && isWallRunning && coyoteTime > 0) WallJump();
        if (CheckCanWallRun())
        {
            if (isWallRunning && currentSpeed < 1) return;

            EnterWallRun();
            
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            Vector3 wallNormal = canRunRight ? -rightWallHit.normal : leftWallHit.normal;
            Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

            rb.AddForce(wallForward * wallRunForce * Time.fixedDeltaTime * generalForceMultiplier, ForceMode.Force);

            if (canRunRight) cameraController.Tilt(cameraController.wallRunTiltAmount, true);
            else if (canRunLeft) cameraController.Tilt(cameraController.wallRunTiltAmount, false);
        }
        else if (!CheckCanWallRun() && allowMovement) ExitWallRun();
    }

    private void WallJump()
    {
        // Get the wall normal, obtain the jump direction force by multiplying each transform direction
        // Lets the player jump off of a wall forwards at a diagonal angle away from the wall normal
        Vector3 wallNormal = canRunRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 jumpDirection = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        // Apply the force, dont need to reset yVelocity as it is already done in HandleWallRunning()
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(jumpDirection * Time.fixedDeltaTime * generalForceMultiplier, ForceMode.Impulse);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.jump);
    }

    private void Slide(InputAction.CallbackContext context)
    {
        ResetCoyoteTime();
        if (new Vector3(moveDirection.x, 0, moveDirection.z) == Vector3.zero) return;
        if (isWallRunning) return;

        isSliding = true;
        currentSpeed = slidingSpeed;

        transform.localScale = new Vector3 (transform.localScale.x, slideHeight, transform.localScale.z);
        rb.AddForce(moveDirection * slideForce * Time.deltaTime, ForceMode.Force);
    }

    private bool CheckCanExitSlide()
    {
        if (Physics.SphereCast(transform.position, crouchCollisionSphereSize, transform.up, out RaycastHit hit, uncrouchCheckDistance, crouchCheckLayer)) return false;
        else
        {
            return true;
        }   
    }

    private void ExitSlide(InputAction.CallbackContext context)
    {
        if (!canExitSlide) 
        {
            attemptingToExitSlide = true;
            return;
        }

        attemptingToExitSlide = false;
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
        playerHUDManager.SetTime(playTime);
        playerHUDManager.SetPlayerSpeed(currentSpeed);
    }

    private void RegisterCallbacks()
    {
        inputHandler.jumpAction.performed += Jump;
        inputHandler.slideAction.performed += Slide;
        inputHandler.slideAction.canceled += ExitSlide;
    }
    public void UnregisterCallbacks() 
    {
        inputHandler.jumpAction.performed -= Jump;
        inputHandler.slideAction.performed -= Slide;
        inputHandler.slideAction.canceled -= ExitSlide;
    }
}
