using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public class DeadMobState : MonoBehaviour, IState<Mob, MobStateId>
{
    // Private fields.

    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // public fields.
    [Header("State Attributes")]
    public MobStateIdConstant stateId;
    public MobStateIdConstant[] nextStateIds;
    public float interval;

    public void BeginState(Mob c, Dictionary<string, object> args = null)
    {
        c.mobAnimator.ResetAllAnimatorTriggers();
        c.mobAnimator.SetTrigger(ANIMATION_TRIGGER_EMOTE_DEAD);

        for (int i = 0; i < c.passiveHitboxes.Length; i++)
            c.passiveHitboxes[i].gameObject.SetActive(false);
    }

    public void FixedUpdateState(Mob c) { }

    public void UpdateState(Mob c)
    {
        if (c.StateTimer > interval && nextStateIds.Length > 0)
        {
            c.ChangeToRandomState(nextStateIds);
            return;
        }
    }

    public void EndState(Mob c) { }
}
