using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInputController : MonoBehaviour
{
    private static GameInputController global;
    public static GameInputController Global
    {
        get
        {
            if (global == null)
            {
                global = GameMasterController.Global.inputController;
            }
            return global;
        }
    }

    public InputActionAsset controls;
    InputActionMap actionMap;

    [NonSerialized] public InputAction actionPositive;
    [NonSerialized] public InputAction actionNegative;
    [NonSerialized] public InputAction actionHorizontal;
    [NonSerialized] public InputAction actionVertical;
    [NonSerialized] public InputAction actionInteract;
    [NonSerialized] public InputAction actionInspect;
    [NonSerialized] public InputAction actionStart;
    [NonSerialized] public InputAction actionSelect;
    [NonSerialized] public InputAction actionAimHorizontal;
    [NonSerialized] public InputAction actionAimVertical;
    [NonSerialized] public InputAction actionAimZoom;
    [NonSerialized] public InputAction actionPositive2;
    [NonSerialized] public InputAction actionNegative2;

    [NonSerialized] public float sensitivityCameraZoom = 0.05f;
    [NonSerialized] public float sensitivityCameraHorizontal = 0.7f;
    [NonSerialized] public float sensitivityCameraVertical = 0.7f;

    [System.NonSerialized] public bool wasInputPositive = false;
    [System.NonSerialized] public bool wasInputNegative = false;
    [System.NonSerialized] public bool wasInputInteract = false;
    [System.NonSerialized] public bool wasInputInspect = false;
    [System.NonSerialized] public bool wasInputPositive2 = false;
    [System.NonSerialized] public bool wasInputNegative2 = false;

    [System.NonSerialized] public bool wasInputStart = false;

    [System.NonSerialized] public bool isInputPositive = false;
    [System.NonSerialized] public bool isInputNegative = false;
    [System.NonSerialized] public bool isInputInteract = false;
    [System.NonSerialized] public bool isInputInspect = false;
    [System.NonSerialized] public bool isInputPositive2 = false;
    [System.NonSerialized] public bool isInputNegative2 = false;

    [System.NonSerialized] public bool isInputStart = false;

    void Awake()
    {
        actionMap = controls.FindActionMap("action_map");

        actionPositive = actionMap.FindAction("positive");
        actionNegative = actionMap.FindAction("negative");
        actionHorizontal = actionMap.FindAction("horizontal");
        actionVertical = actionMap.FindAction("vertical");
        actionInteract = actionMap.FindAction("interact");
        actionInspect = actionMap.FindAction("inspect");
        actionStart = actionMap.FindAction("start");
        actionSelect = actionMap.FindAction("select");
        actionAimHorizontal = actionMap.FindAction("aim_horizontal");
        actionAimVertical = actionMap.FindAction("aim_vertical");
        actionAimZoom = actionMap.FindAction("aim_zoom");
        actionPositive2 = actionMap.FindAction("positive_2");
        actionNegative2 = actionMap.FindAction("negative_2");

        actionPositive.Enable();
        actionNegative.Enable();
        actionHorizontal.Enable();
        actionVertical.Enable();
        actionInteract.Enable();
        actionInspect.Enable();
        actionStart.Enable();
        actionSelect.Enable();
        actionAimHorizontal.Enable();
        actionAimVertical.Enable();
        actionAimZoom.Enable();

        actionPositive2.Enable();
        actionNegative2.Enable();
    }

    void Update()
    {
        wasInputPositive = isInputPositive;
        wasInputNegative = isInputNegative;
        wasInputInteract = isInputInteract;
        wasInputInspect = isInputInspect;
        wasInputPositive2 = isInputPositive2;
        wasInputNegative2 = isInputNegative2;

        wasInputStart = isInputStart;

        isInputPositive = actionPositive.ReadValue<float>() >= 0.1f;
        isInputNegative = actionNegative.ReadValue<float>() >= 0.1f;
        isInputInteract = actionInteract.ReadValue<float>() >= 0.1f;
        isInputInspect = actionInspect.ReadValue<float>() >= 0.1f;
        isInputPositive2 = actionPositive2.ReadValue<float>() >= 0.1f;
        isInputNegative2 = actionNegative2.ReadValue<float>() >= 0.1f;
        isInputStart = actionStart.ReadValue<float>() >= 0.1f;
    }
}
