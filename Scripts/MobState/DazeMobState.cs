using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public class DazeMobState : MonoBehaviour, IState<Mob, MobStateId>
{
    // Private fields.
    private float stateInterval;

    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // public fields.
    [Header("State Attributes")]
    public MobStateIdConstant stateId;
    public MobStateIdConstant[] nextStateIds;
    public float minInterval;
    public float maxInterval;


    public void BeginState(Mob c, Dictionary<string, object> args = null)
    {
        c.mobAnimator.ResetAllAnimatorTriggers();
        c.mobAnimator.SetTrigger(ANIMATION_TRIGGER_EMOTE_DAZE);
        stateInterval = Random.Range(minInterval, maxInterval);
    }

    public void FixedUpdateState(Mob c) { }

    public void UpdateState(Mob c)
    {
        if(c.StateTimer > stateInterval)
        {
            c.ChangeToRandomState(nextStateIds);
            return;
        }
    }

    public void EndState(Mob c) 
    {
    }
}
