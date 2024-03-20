using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class BeginFixedCamcorderAction : MonoBehaviour, IAction
{
    // Private fields.
    private float fixedTransitionTimer;

    // Public properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.BeginFixedCamcorder;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => doWaitForTransition 
        ? fixedTransitionTimer >= fixedTransitionInterval 
        : true;
    public bool IsActionUpdateComplete => doWaitForTransition 
        ? fixedTransitionTimer >= fixedTransitionInterval 
        : true;

    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Fixed Attributes")]
    public GameObject fixedPositionObject;
    public float fixedTransitionInterval;
    public bool doWaitForTransition;

    public void BeginAction(ActionSource actionSource)
    {
        fixedTransitionTimer = 0.0F;

        var args = new Dictionary<string, object>();
        args[CAMCORDER_STATE_ARG_FIXED_POSITION_OBJECT] = fixedPositionObject;
        args[CAMCORDER_STATE_ARG_FIXED_TRANSITION_INTERVAL] = fixedTransitionInterval;
        ActiveSceneHighLogic.G.CachedCamcorder.ChangeState(CamcorderStateId.Fixed, args);
    }

    public void UpdateAction(ActionSource actionSource) 
    {
        if (ActionHighLogic.G.IsSkipping)
        {
            var args = new Dictionary<string, object>();
            args[CAMCORDER_STATE_ARG_FIXED_POSITION_OBJECT] = fixedPositionObject;
            args[CAMCORDER_STATE_ARG_FIXED_TRANSITION_INTERVAL] = 0.0F;
            ActiveSceneHighLogic.G.CachedCamcorder.ChangeState(CamcorderStateId.Fixed, args);
            fixedTransitionTimer = fixedTransitionInterval;
        }

        fixedTransitionTimer += Time.deltaTime;
    }

    public void EndAction(ActionSource actionSource) { }
}
