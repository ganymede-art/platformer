using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public class SpottedPlayerMobState : MonoBehaviour, IState<Mob, MobStateId>
{
    // Private fields.
    private Vector3 facingDirection;

    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // Public fields.
    [Header("State Attributes")]
    public MobStateIdConstant stateId;
    public MobStateIdConstant[] nextStateIds;
    public float interval;

    public void BeginState(Mob c, Dictionary<string, object> args = null)
    {
        c.mobAnimator.ResetAllAnimatorTriggers();
        c.mobAnimator.SetTrigger(ANIMATION_TRIGGER_EMOTE_REACT_ALERT);
    }

    public void FixedUpdateState(Mob c) { }

    public void UpdateState(Mob c)
    {
        if(c.StateTimer > interval)
        {
            c.ChangeToRandomState(nextStateIds);
            return;
        }

        facingDirection
            = (ActiveSceneHighLogic.G.CachedPlayerObject.transform.position
            - c.transform.position).normalized;

        MobStatics.UpdateInternalDirection(c, facingDirection);
        MobStatics.UpdateRendererDirection(c, facingDirection);
    }

    public void EndState(Mob c) { }
}
