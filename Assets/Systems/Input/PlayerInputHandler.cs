using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance { get; private set; }

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;
    private InputActionMap defaultActionMap;

    [Header("Action Map Names")]
    [SerializeField] private string defaultActionMapName;

    [Header("Action Names")]
    [SerializeField] private string move;
    [SerializeField] private string look;
    [SerializeField] private string jump;
    [SerializeField] private string slide;
    [SerializeField] private string pause;

    public PlayerInput playerInputComponent;
    
    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;
    public InputAction slideAction;
    public InputAction pauseAction;

    public Vector2 MoveInputVector { get; private set; }
    public Vector2 LookInputVector { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SlideTriggered { get; private set; }
    public bool PauseTriggered { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        defaultActionMap = playerControls.FindActionMap(defaultActionMapName);

        moveAction = defaultActionMap.FindAction(move);
        lookAction = defaultActionMap.FindAction(look);
        jumpAction = defaultActionMap.FindAction(jump);
        slideAction = defaultActionMap.FindAction(slide);
        pauseAction = defaultActionMap.FindAction(pause);

        playerInputComponent = GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        slideAction.Enable();
        pauseAction.Enable();

        AddContexts();
    }
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        slideAction.Disable();
        pauseAction.Disable();
    }

    private void AddContexts()
    {
        moveAction.performed += context => MoveInputVector = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInputVector = Vector2.zero;

        lookAction.performed += context => LookInputVector = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookInputVector = Vector2.zero;

        jumpAction.performed += context => JumpTriggered = true;
        jumpAction.canceled += context => JumpTriggered = false;

        slideAction.performed += context => SlideTriggered = true;
        slideAction.canceled += context => SlideTriggered = false;

        pauseAction.performed += context => PauseTriggered = true;
        pauseAction.canceled += context => PauseTriggered = false;
    }
}
