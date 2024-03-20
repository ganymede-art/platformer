using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class BeginOrbitCamcorderAction : MonoBehaviour, IAction
{
    // Public properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.BeginOrbitCamcorder;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    public void BeginAction(ActionSource actionSource)
    {
        ActiveSceneHighLogic.G.CachedCamcorder.ChangeState(CamcorderStateId.Orbit);
    }

    public void UpdateAction(ActionSource actionSource) { }

    public void EndAction(ActionSource actionSource) { }
}
