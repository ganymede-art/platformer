using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public class JumpRandomDirectionMobState : MonoBehaviour, IState<Mob, MobStateId>
{
    // Consts.
    const float MIN_JUMP_UP_INTERVAL = 0.25F;

    // Private fields.
    private bool isFalling;
    private GroundCheckMobBehaviour groundCheck;
    private Vector3 force;

    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // public fields.
    [Header("State Attributes")]
    public MobStateIdConstant stateId;
    public MobStateIdConstant[] nextStateIds;

    [Header("Force Attributes")]
    public float minHorizontalForceMult;
    public float maxHorizontalForceMult;
    public float minVerticalForceMult;
    public float maxVerticalForceMult;

    public void BeginState(Mob c, Dictionary<string, object> args = null)
    {
        isFalling = false;

        float forceMultX = Random.Range(-1.0F, 1.0F);
        float forceMultZ = Random.Range(-1.0F, 1.0F);

        force = new Vector3(forceMultX, 0.0F, forceMultZ).normalized;
        force *= Random.Range(minHorizontalForceMult, maxHorizontalForceMult);
        force.y = Random.Range(minVerticalForceMult, maxVerticalForceMult);

        c.mobRigidBody.AddForce(force, ForceMode.VelocityChange);

        c.mobAnimator.ResetAllAnimatorTriggers();
        c.mobAnimator.SetTrigger(ANIMATION_TRIGGER_JUMP_UP);

        groundCheck = MobStatics.GetMobBehaviour<GroundCheckMobBehaviour>(c, MobBehaviourId.GroundCheck);
    }

    public void EndState(Mob c)
    {
        
    }

    public void FixedUpdateState(Mob c)
    {
        
    }

    public void UpdateState(Mob c)
    {
        if(!isFalling && c.StateTimer > MIN_JUMP_UP_INTERVAL && c.mobRigidBody.velocity.y <= 0.0F)
        {
            isFalling = true;
            c.mobAnimator.ResetAllAnimatorTriggers();
            c.mobAnimator.SetTrigger(ANIMATION_TRIGGER_JUMP_DOWN);
        }

        if (isFalling && groundCheck.IsSphereCastGrounded)
            c.ChangeToRandomState(nextStateIds, null);

        // Renderer.
        MobStatics.UpdateInternalDirection(c, force);
        MobStatics.UpdateRendererDirection(c, force, ANIMATION_TURNING_SPEED_MULT_SLOW);
    }
}
