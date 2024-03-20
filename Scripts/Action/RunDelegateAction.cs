using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunDelegateAction : MonoBehaviour, IAction
{
    public GameObject NextActionObject => nextActionObjectFunction == null 
        ? nextActionObject : nextActionObjectFunction.Invoke();

    public ActionType ActionType => ActionType.RunDelegate;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => isActionCompleteFunction == null 
        ? true : isActionCompleteFunction.Invoke();
    public bool IsActionUpdateComplete => isActionUpdateCompleteFunction == null 
        ? true : isActionUpdateCompleteFunction.Invoke();

    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    public Func<GameObject> nextActionObjectFunction = null;
    public Func<bool> isActionCompleteFunction = null;
    public Func<bool> isActionUpdateCompleteFunction = null;
    public Action<ActionSource> beginActionDelegate = null;
    public Action<ActionSource> updateActionDelegate = null;
    public Action<ActionSource> endActionDelegate = null;

    public void BeginAction(ActionSource actionSource)
    {
        if (beginActionDelegate != null)
            beginActionDelegate.Invoke(actionSource);
    }

    public void UpdateAction(ActionSource actionSource)
    {
        if (updateActionDelegate != null)
            updateActionDelegate.Invoke(actionSource);
    }

    public void EndAction(ActionSource actionSource)
    {
        if (endActionDelegate != null)
            endActionDelegate.Invoke(actionSource);
    }
}
