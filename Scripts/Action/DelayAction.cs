using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAction : MonoBehaviour, IAction
{
    // Private fields.
    private bool isDelayComplete;

    // Public properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.Delay;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => isDelayComplete;
    public bool IsActionUpdateComplete => isDelayComplete;

    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Delay Attributes")]
    public float delayInterval;

    public void BeginAction(ActionSource actionSource)
    {
        isDelayComplete = false;
    }

    public void UpdateAction(ActionSource actionSource)
    {
        if (actionSource.actionTimer > delayInterval)
            isDelayComplete = true;
    }

    public void EndAction(ActionSource actionSource) { }
}
