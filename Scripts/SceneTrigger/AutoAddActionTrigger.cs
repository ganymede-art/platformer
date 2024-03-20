using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutoAddActionTrigger : MonoBehaviour, INameable
{
    private bool isFired;
    public AddActionHighLogicTrigger highLogicTrigger;

    void Start()
    {
        if (highLogicTrigger == null)
        {
            Debug.LogWarning($"[{GetType()}] high logic trigger is missing.");
            enabled = false;
            return;
        }
        
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if (isFired)
            return;

#if UNITY_EDITOR
        if (Keyboard.current.digit6Key.isPressed)
        {
            enabled = false;
            return;
        }
#endif

        isFired = true;
        highLogicTrigger.AddAction();
        enabled = false;
    }

    public string GetName() => $"AutoAddActionTrigger{highLogicTrigger?.actionId}";
}
