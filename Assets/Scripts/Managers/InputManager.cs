using UnityEngine;
using UnityEngine.InputSystem;

//Manages the high level input states of the game, such as aim direction of the player, and whether player is attacking. 
//This usually requires processing of lower level inputs such as converstion of raw mouse position, which we will help handle here.

public enum InputMode { KeyboardMouse, Touchscreen }

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public InputMode currentInputMode { get; private set; }

    public InputActionReference lookInputAction;

    public bool isAttacking { get; private set; } //detects if right game pad or mouse button is being presseds
    public GameObject touchScreenControls;
    public Vector2 aimDirection { get; private set; }
    public Transform playerPosition;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Determine the current input mode based on available devices
        var device = InputSystem.GetDevice<InputDevice>();
        if (device is Gamepad || device is Touchscreen)
        {
            currentInputMode = InputMode.Touchscreen;
            if (touchScreenControls != null)
            {
                touchScreenControls.SetActive(true);
            }
        }
        else
        {
            currentInputMode = InputMode.KeyboardMouse;
        }
    }

    void Update()
    {
        UpdateAimDirection();
    }

    public void UpdateAimDirection()
    {

        if (currentInputMode == InputMode.Touchscreen)
        {
            aimDirection = lookInputAction.action.ReadValue<Vector2>();
            if (lookInputAction.action.WasPressedThisFrame())
            {
                isAttacking = true;
            }
            else if (lookInputAction.action.WasReleasedThisFrame())
            {
                isAttacking = false;
            }
            return;
        }
        else
        {
            if (Camera.main == null || Mouse.current == null)
            {
                aimDirection = Vector2.zero;
                print("Camera.main or Mouse.current is null in InputManager.UpdateAimDirection");
                return;
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 rawAimDirection = (mousePosition - playerPosition.position);
            aimDirection = rawAimDirection.sqrMagnitude > 0.0001f ? rawAimDirection.normalized : Vector2.zero;
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                isAttacking = true;
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                isAttacking = false;
            }
            return;
        }
    }
}
