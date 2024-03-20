using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public GameObject NextActionObject { get; }
    public ActionType ActionType { get; }
    public string ActionName { get; }
    public bool IsActionComplete { get; }
    public bool IsActionUpdateComplete { get; }

    public void BeginAction(ActionSource actionSource);
    public void UpdateAction(ActionSource actionSource);
    public void EndAction(ActionSource actionSource);
}