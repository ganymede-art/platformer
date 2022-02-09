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

    [NonSerialized] public InputAction buttonNorth;
    [NonSerialized] public InputAction buttonEast;
    [NonSerialized] public InputAction buttonSouth;
    [NonSerialized] public InputAction buttonWest;

    [NonSerialized] public InputAction buttonWestExtra;
    [NonSerialized] public InputAction buttonEastExtra;

    [NonSerialized] public InputAction axisMoveHorizontal;
    [NonSerialized] public InputAction axisMoveVertical;

    [NonSerialized] public InputAction axisAimHorizontal;
    [NonSerialized] public InputAction axisAimVertical;

    [NonSerialized] public InputAction buttonStart;
    [NonSerialized] public InputAction buttonSelect;

    [NonSerialized] public InputAction axisZoom;

    [NonSerialized] public float sensitivityCameraZoom = 0.05F;
    [NonSerialized] public float sensitivityCameraHorizontal = 0.7F;
    [NonSerialized] public float sensitivityCameraVertical = 0.7F;

    [System.NonSerialized] public bool wasInputNorth = false;
    [System.NonSerialized] public bool wasInputEast = false;
    [System.NonSerialized] public bool wasInputSouth = false;
    [System.NonSerialized] public bool wasInputWest = false;
    
    [System.NonSerialized] public bool wasInputEastExtra = false;
    [System.NonSerialized] public bool wasInputWestExtra = false;

    [System.NonSerialized] public bool wasInputStart = false;

    [System.NonSerialized] public bool isInputNorth = false;
    [System.NonSerialized] public bool isInputEast = false;
    [System.NonSerialized] public bool isInputSouth = false;
    [System.NonSerialized] public bool inInputWest = false;
    
    [System.NonSerialized] public bool isInputEastExtra = false;
    [System.NonSerialized] public bool isInputWestExtra = false;

    [System.NonSerialized] public bool isInputStart = false;

    void Awake()
    {
        actionMap = controls.FindActionMap("action_map");

        buttonNorth = actionMap.FindAction("north");
        buttonEast = actionMap.FindAction("east");
        buttonSouth = actionMap.FindAction("south");
        buttonWest = actionMap.FindAction("west");

        buttonEastExtra = actionMap.FindAction("east_extra");
        buttonWestExtra = actionMap.FindAction("west_extra");

        axisMoveHorizontal = actionMap.FindAction("move_horizontal");
        axisMoveVertical = actionMap.FindAction("move_vertical");
        axisAimHorizontal = actionMap.FindAction("aim_horizontal");
        axisAimVertical = actionMap.FindAction("aim_vertical");

        buttonStart = actionMap.FindAction("start");
        buttonSelect = actionMap.FindAction("select");

        axisZoom = actionMap.FindAction("zoom");

        buttonNorth.Enable();
        buttonEast.Enable();
        buttonSouth.Enable();        
        buttonWest.Enable();

        buttonEastExtra.Enable();
        buttonWestExtra.Enable();

        axisMoveHorizontal.Enable();
        axisMoveVertical.Enable();

        axisAimHorizontal.Enable();
        axisAimVertical.Enable();

        buttonStart.Enable();
        buttonSelect.Enable();
        
        axisZoom.Enable();
    }

    void Update()
    {
        wasInputNorth = isInputNorth;
        wasInputEast = isInputEast;
        wasInputSouth = isInputSouth;
        wasInputWest = inInputWest;
        
        wasInputEastExtra = isInputEastExtra;
        wasInputWestExtra = isInputWestExtra;

        wasInputStart = isInputStart;

        isInputNorth = buttonNorth.ReadValue<float>() >= 0.1F;
        isInputEast = buttonEast.ReadValue<float>() >= 0.1F;
        isInputSouth = buttonSouth.ReadValue<float>() >= 0.1F;
        inInputWest = buttonWest.ReadValue<float>() >= 0.1F;
        
        isInputEastExtra = buttonEastExtra.ReadValue<float>() >= 0.1F;
        isInputWestExtra = buttonWestExtra.ReadValue<float>() >= 0.1F;

        isInputStart = buttonStart.ReadValue<float>() >= 0.1F;

        
    }

    
}
