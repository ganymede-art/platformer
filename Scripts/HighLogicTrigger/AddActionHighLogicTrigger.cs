using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AddActionHighLogicTrigger : MonoBehaviour
{
    // Private fields.
    private bool isFired;

    // Public fields.
    [Header("Action Attributes")]
    [ContextMenuItem("Set Random Id", "SetRandomWidgetId")]
    public string actionId;
    public HighLogicStateIdConstant actionHighLogicStateId;
    public GameObject activeActionObject;

    [Header("Add Action Attributes")]
    public bool isSequenced;

    [Header("Variable Attributes")]
    public bool isOneShot;
    public VariableIdConstant oneShotVariableId;
    public bool doSetOneShot;

    // Context methods.
    private void SetRandomWidgetId() => actionId = Guid.NewGuid().ToString();

    public void AddAction()
    {
        if(isOneShot && oneShotVariableId != null)
        {
            bool isOneShotSet = PersistenceHighLogic.G.GetBoolVariable(oneShotVariableId.VariableId);
            if (isOneShotSet)
                return;
        }

        if (isOneShot && oneShotVariableId == null && isFired )
            return;

        if (actionId == string.Empty)
            return;

        var actionSource = new ActionSource();
        actionSource.actionId = actionId;
        actionSource.actionHighLogicStateId = actionHighLogicStateId.highLogicStateId;
        actionSource.actionStatus = ActionStatus.None;
        actionSource.activeActionObject = activeActionObject;

        if (isSequenced)
            ActionHighLogic.G.AddSequencedAction(actionSource);
        else
            ActionHighLogic.G.AddConcurrentAction(actionSource);

        if (doSetOneShot && oneShotVariableId != null)
            PersistenceHighLogic.G.SetBoolVariable(oneShotVariableId.VariableId, true);

        isFired = true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AddActionHighLogicTrigger))]
public class GameEventContainerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AddActionHighLogicTrigger trigger = (AddActionHighLogicTrigger)target;

        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Activate Trigger"))
            {
                trigger.AddAction();
            }
        }
    }
}
#endif