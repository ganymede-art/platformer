using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddActionAction : MonoBehaviour, IAction
{
    // Public Properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.AddAction;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;
    public AddActionHighLogicTrigger highLogicTrigger;

    public void BeginAction(ActionSource actionSource)
    {
        highLogicTrigger.AddAction();
    }

    public void EndAction(ActionSource actionSource) { }
    public void UpdateAction(ActionSource actionSource) { }
}
