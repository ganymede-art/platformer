using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Constants;

public class InputHighLogic : MonoBehaviour
{
    const float BUTTON_THRESHOLD = 0.25F;

    const float MOUSE_USING_THRESHOLD = 0.1F;
    const float MOUSE_X_SENSITIVITY = 0.1F;
    const float MOUSE_Y_SENSITIVITY = 0.1F;

    const string INPUT_ACTION_MAP_NAME = "ActionMap";

    const string ACTION_NAME_MOVE = "Move";
    const string ACTION_NAME_LOOK = "Look";
    const string ACTION_NAME_ZOOM = "Zoom";
    const string ACTION_NAME_MOUSE_LOOK = "MouseLook";
    const string ACTION_NAME_NORTH = "North";
    const string ACTION_NAME_EAST = "East";
    const string ACTION_NAME_SOUTH = "South";
    const string ACTION_NAME_WEST = "West";
    const string ACTION_NAME_SELECT = "Select";
    const string ACTION_NAME_START = "Start";
    const string ACTION_NAME_NEAR = "Near";
    const string ACTION_NAME_FAR = "Far";

    private InputActionAsset inputActionAsset;
    private InputActionMap inputActionMap;

    private InputAction moveStick;
    private InputAction lookStick;
    private InputAction zoomAxis;

    private InputAction northButton;
    private InputAction eastButton;
    private InputAction southButton;
    private InputAction westButton;
    private InputAction selectButton;
    private InputAction startButton;
    private InputAction nearButton;
    private InputAction farButton;

    private Vector2 move2d;
    private Vector3 move3d;
    private Vector2 look2d;
    private Vector3 look3d;
    private float zoom;

    private bool wasNorthPressed;
    private bool wasEastPressed;
    private bool wasSouthPressed;
    private bool wasWestPressed;
    private bool wasSelectPressed;
    private bool wasStartPressed;
    private bool wasNearPressed;
    private bool wasFarPressed;

    private bool wasUpPressed;
    private bool wasDownPressed;

    private bool isNorthPressed;
    private bool isEastPressed;
    private bool isSouthPressed;
    private bool isWestPressed;
    private bool isSelectPressed;
    private bool isStartPressed;
    private bool isNearPressed;
    private bool isFarPressed;

    private bool isUpPressed;
    private bool isDownPressed;

    private bool isUsingMouse;
    private float mouseX;
    private float mouseY;

    // Public properties.
    public static InputHighLogic G => GameHighLogic.G?.InputHighLogic;

    public bool IsInputActive => StateHighLogic.G.StateTimer > INPUT_STATE_BLANK_INTERVAL;

    public Vector2 Move2d => move2d;
    public Vector3 Move3d => move3d;
    public Vector2 Look2d => look2d;
    public Vector3 Look3d => look3d;

    public float Zoom => zoom;

    public bool WasNorthPressed => wasNorthPressed;
    public bool WasEastPressed => wasEastPressed;
    public bool WasSouthPressed => wasSouthPressed;
    public bool WasWestPressed => wasWestPressed;
    public bool WasSelectPressed => wasSelectPressed;
    public bool WasStartPressed => wasStartPressed;
    public bool WasNearPressed => wasNearPressed;
    public bool WasFarPressed => wasFarPressed;

    public bool WasUpPressed => wasUpPressed;
    public bool WasDownPressed => wasDownPressed;

    public bool IsNorthPressed => isNorthPressed;
    public bool IsEastPressed => isEastPressed;
    public bool IsSouthPressed => isSouthPressed;
    public bool IsWestPressed => isWestPressed;
    public bool IsSelectPressed => isSelectPressed;
    public bool IsStartPressed => isStartPressed;
    public bool IsNearPressed => isNearPressed;
    public bool IsFarPressed => isFarPressed;

    public bool IsUpPressed => isUpPressed;
    public bool IsDownPressed => isDownPressed;

    public bool IsUsingMouse => isUsingMouse;
    public float MouseX => mouseX;
    public float MouseY => mouseY;

    public bool IsMove3dPressed => move3d.magnitude > INPUT_THRESHOLD_AXIS;

    private void Awake()
    {
        inputActionAsset = Resources.Load<InputActionAsset>(RESOURCE_PATH_INPUT_ACTION_ASSET);
        inputActionMap = inputActionAsset.FindActionMap(INPUT_ACTION_MAP_NAME);

        moveStick = inputActionMap.FindAction(ACTION_NAME_MOVE);
        lookStick = inputActionMap.FindAction(ACTION_NAME_LOOK);

        zoomAxis = inputActionMap.FindAction(ACTION_NAME_ZOOM);

        northButton = inputActionMap.FindAction(ACTION_NAME_NORTH);
        eastButton = inputActionMap.FindAction(ACTION_NAME_EAST);
        southButton = inputActionMap.FindAction(ACTION_NAME_SOUTH);
        westButton = inputActionMap.FindAction(ACTION_NAME_WEST);

        selectButton = inputActionMap.FindAction(ACTION_NAME_SELECT);
        startButton = inputActionMap.FindAction(ACTION_NAME_START);

        nearButton = inputActionMap.FindAction(ACTION_NAME_NEAR);
        farButton = inputActionMap.FindAction(ACTION_NAME_FAR);

        foreach (var action in inputActionMap.actions)
            action.Enable();

        move2d = Vector2.zero;
        move3d = Vector3.zero;

        look2d = Vector2.zero;
        look3d = Vector3.zero;
    }

    private void Update()
    {
        move2d = moveStick.ReadValue<Vector2>();

        move3d.x = moveStick.ReadValue<Vector2>().x;
        move3d.z = moveStick.ReadValue<Vector2>().y;
        move3d.y = 0.0F;

        look2d = lookStick.ReadValue<Vector2>();

        look3d.x = lookStick.ReadValue<Vector2>().x;
        look3d.z = lookStick.ReadValue<Vector2>().y;
        look3d.y = 0.0F;

        zoom = zoomAxis.ReadValue<float>();

        wasNorthPressed = isNorthPressed;
        wasEastPressed = isEastPressed;
        wasSouthPressed = isSouthPressed;
        wasWestPressed = isWestPressed;
        wasSelectPressed = isSelectPressed;
        wasStartPressed = isStartPressed;
        wasNearPressed = isNearPressed;
        wasFarPressed = isFarPressed;

        wasUpPressed = isUpPressed;
        wasDownPressed = isDownPressed;

        isNorthPressed = northButton.ReadValue<float>() > BUTTON_THRESHOLD;
        isEastPressed = eastButton.ReadValue<float>() > BUTTON_THRESHOLD;
        isSouthPressed = southButton.ReadValue<float>() > BUTTON_THRESHOLD;
        isWestPressed = westButton.ReadValue<float>() > BUTTON_THRESHOLD;
        isSelectPressed = selectButton.ReadValue<float>() > BUTTON_THRESHOLD;
        isStartPressed = startButton.ReadValue<float>() > BUTTON_THRESHOLD;
        isNearPressed = nearButton.ReadValue<float>() > BUTTON_THRESHOLD;
        isFarPressed = farButton.ReadValue<float>() > BUTTON_THRESHOLD;

        isUpPressed = move2d.y > BUTTON_THRESHOLD;
        isDownPressed = move2d.y < -BUTTON_THRESHOLD;

        isUsingMouse = Mouse.current.delta.magnitude >= MOUSE_USING_THRESHOLD;
        mouseX = Mouse.current.delta.x.ReadValue() * MOUSE_X_SENSITIVITY;
        mouseY = Mouse.current.delta.y.ReadValue() * MOUSE_Y_SENSITIVITY;
    }

    private void OnGUI()
    {
        string debug
            = $"\nIs SENW:"
            + $"\n    S|{isNorthPressed}"
            + $"\n    S|{WasSouthPressed}"
            + $"\nMouse: {mouseX}, {mouseY}";
        GUI.Label(new Rect(500, 0, 500, 500), debug);
    }
}
