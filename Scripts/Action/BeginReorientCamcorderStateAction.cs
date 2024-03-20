using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginReorientCamcorderState : MonoBehaviour, IAction
{
    // Public properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.BeginReorientCamcorder;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    public void BeginAction(ActionSource actionSource)
    {
        ActiveSceneHighLogic.G.CachedCamcorder.ChangeState(CamcorderStateId.Reorient);
    }

    public void UpdateAction(ActionSource actionSource) { }

    public void EndAction(ActionSource actionSource) { }
}
