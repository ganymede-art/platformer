using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public class StampedeWallMobState : MonoBehaviour, IState<Mob,MobStateId>
{
    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // public fields.
    [Header("State Attributes")]
    public MobStateIdConstant stateId;
    public MobStateIdConstant[] nextStateIds;
    public float interval;

    [Header("Stampede Wall Attributes")]
    public float forceMult;

    public void BeginState(Mob c, Dictionary<string, object> args = null)
    {
        c.mobAnimator.ResetAllAnimatorTriggers();
        c.mobAnimator.SetTrigger(ANIMATION_TRIGGER_HURT_DOWN);

        c.mobRigidBody.velocity = Vector3.zero;
        c.mobRigidBody.AddForce(-c.mobDirectionObject.transform.forward * forceMult, ForceMode.VelocityChange);
        c.mobRigidBody.AddForce(Vector3.up * forceMult, ForceMode.VelocityChange);
    }

    public void FixedUpdateState(Mob c) { }

    public void UpdateState(Mob c)
    {
        if (c.StateTimer > interval)
        {
            c.ChangeToRandomState(nextStateIds);
            return;
        }
    }

    public void EndState(Mob c) 
    {
    }
}
