using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolVariableAction : MonoBehaviour, IAction
{
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.SetBoolVariable;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Variable Attributes")]
    public VariableIdConstant variableId;
    public bool variableValue;

    public void BeginAction(ActionSource actionSource)
    {
        PersistenceHighLogic.G.SetBoolVariable(variableId.VariableId, variableValue);
    }

    public void EndAction(ActionSource actionSource) { }

    public void UpdateAction(ActionSource actionSource) { }
}
