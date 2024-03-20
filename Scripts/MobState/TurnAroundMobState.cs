using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public class TurnAroundMobState : MonoBehaviour, IState<Mob, MobStateId>
{
    // Private fields.
    Vector3 turnAroundDirection;

    // Public Properties.
    public MobStateId StateId => stateId.mobStateId;

    // Public fields.
    [Header("State Attributes")]
    public MobStateIdConstant stateId;
    public MobStateIdConstant[] nextStateIds;
    public float interval;

    public void BeginState(Mob c, Dictionary<string, object> args = null)
    {
        c.mobAnimator.ResetAllAnimatorTriggers();
        c.mobAnimator.SetTrigger(ANIMATION_TRIGGER_SIDLE);

        turnAroundDirection = -c.mobDirectionObject.transform.forward;
    }

    public void FixedUpdateState(Mob c) { }

    public void UpdateState(Mob c)
    {
        if (c.StateTimer > interval)
        {
            c.ChangeToRandomState(nextStateIds);
            return;
        }

        MobStatics.UpdateInternalDirection(c, turnAroundDirection);
        MobStatics.UpdateRendererDirection(c, turnAroundDirection, ANIMATION_TURNING_SPEED_MULT_SLOW);
    }

    public void EndState(Mob c) { }
}
