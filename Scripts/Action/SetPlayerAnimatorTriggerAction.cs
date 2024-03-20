using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class SetPlayerAnimatorTriggerAction : MonoBehaviour, IAction
{ 
    // Public properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.SetPlayerAnimatorTrigger;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;
    [Space]
    public AnimatorTriggerIdConstant animatorTriggerId;
    public bool doSetSpeedMultiplier;
    public float speedMultiplier;

    public void BeginAction(ActionSource actionSource)
    {
        ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.ResetAllAnimatorTriggers();
        ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.SetTrigger(animatorTriggerId.AnimatorTriggerId);
        if (doSetSpeedMultiplier)
            ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.SetFloat(ANIMATION_TRIGGER_SPEED_MULTIPLIER, speedMultiplier);
    }

    public void EndAction(ActionSource actionSource) { }
    public void UpdateAction(ActionSource actionSource) { }
}
