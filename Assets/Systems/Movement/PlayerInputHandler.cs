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

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction slideAction;

    public Vector2 moveInputVector { get; private set; }
    public Vector2 lookInputVector { get; private set; }
    public bool jumpTriggered { get; private set; }
    public bool slideTriggered { get; private set; }

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
        //slideAction = defaultActionMap.FindAction(slide);
    }
    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        //slideAction.Enable();

        AddContexts();
    }
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        //slideAction.Disable();
    }

    private void AddContexts()
    {
        moveAction.performed += context => moveInputVector = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInputVector = Vector2.zero;

        lookAction.performed += context => lookInputVector = context.ReadValue<Vector2>();
        lookAction.canceled += context => lookInputVector = Vector2.zero;

        jumpAction.performed += context => jumpTriggered = true;
        jumpAction.canceled += context => jumpTriggered = false;

        //slideAction.performed += context => slideTriggered = true;
        //slideAction.canceled += context => slideTriggered = false;
    }
}
