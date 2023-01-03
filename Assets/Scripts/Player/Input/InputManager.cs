using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Singleton
    
    static InputManager _instance;

    public static InputManager Instance 
    {
        get 
        {
            return _instance;
        }
    }

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    #endregion

    PlayerControls controls;

    // This function is called when the object becomes enabled and active.
    void OnEnable()
    {
        controls = new PlayerControls();
        controls.Enable();
    }

    // This function is called when the behaviour becomes disabled or inactive.
    void OnDisable()
    {
        controls.Disable();
    }

    // Get the movement from right to left
    public float GetMovement() 
    {
        return controls.Player.Movement.ReadValue<float>();
    }

    // Returns true if the jump button was pressed this frame
    public bool GetJumpPressed()
    {
        return controls.Player.Jump.WasPressedThisFrame();
    }

    // Returns true if the jump button was released this frame
    public bool GetJumpReleased()
    {
        return controls.Player.Jump.WasReleasedThisFrame();
    }

    // Returns true if the restart button was released this frame
    public bool GetRestartPressed()
    {
        return controls.Player.Restart.WasPressedThisFrame();
    }

    // Returns true if the climb button was pressed this frame
    public bool GetClimbPressed()
    {
        return controls.Player.Climb.WasPressedThisFrame();
    }

    // Returns true if the climb button was released this frame
    public bool GetClimbReleased()
    {
        return controls.Player.Climb.WasReleasedThisFrame();
    }

    // Returns true if the fire button was pressed this frame
    public bool GetFirePressed()
    {
        return controls.Player.Fire.WasPressedThisFrame();
    }

    // Returns the position of the cursor in this frame
    public Vector2 GetCursorPosition()
    {
        return controls.Player.CursorPosition.ReadValue<Vector2>();
    }

    // Returns if the player wants to interact
    public bool GetInteract()
    {
        return controls.Player.Interact.WasPressedThisFrame();
    }

    public bool GetEscape() 
    {
        return controls.Player.Escape.WasPressedThisFrame();
    }
}