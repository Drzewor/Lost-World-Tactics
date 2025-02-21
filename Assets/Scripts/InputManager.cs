#define USE_NEW_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {get; private set;}
    private PlayerInputActions playerInputActions;

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogError($"There is more than one InputManager {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public bool IsMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    public bool IsESCButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.OpenMenu.WasPressedThisFrame();
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }

    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        Vector2 inputMoveDirection = new Vector2(0,0);

        if(Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.y = +1f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.y = -1f;
        }
        if(Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }
        if(Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = +1f;
        }

        return inputMoveDirection;
#endif
    }

    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
        float rotateAmount = 0f;

        if(Input.GetKey(KeyCode.Q))
        {
            rotateAmount = +1f;
        }
        if(Input.GetKey(KeyCode.E))
        {
            rotateAmount = -1f;
        }

        return rotateAmount;
#endif
    }

    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
        float zoomAmount = 0;

        if(Input.mouseScrollDelta.y > 0)
        {
            zoomAmount = -1f;
        }
        if(Input.mouseScrollDelta.y < 0)
        {
            zoomAmount = +1f;
        }

        return zoomAmount;
#endif
    }

    public void DisablePlayerInputActions()
    {
        playerInputActions.Player.Disable();
    }
}
