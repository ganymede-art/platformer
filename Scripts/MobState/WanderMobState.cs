using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public class WanderMobState : MonoBehaviour, IState<Mob, MobStateId>
{
    // Private fields.
    private float stateInterval;
    private Vector3 wanderDirection;
    private Vector3 wanderTargetDirection;
    private WallCheckMobBehaviour wallCheck;
    private bool didBeginStateHittingWall;

    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // public fields.
    [Header("State Attributes")]
    public MobStateIdConstant stateId;
    public MobStateIdConstant[] nextStateIds;
    public float minInterval;
    public float maxInterval;
    public MobStateIdConstant[] onHitWallStateIds;

    [Header("Wander Attributes")]
    public float wanderForceMult;
    public float wanderMaxVel;

    [Header("Animation Attributes")]
    public bool doUseVelAnimSpeedMult;
    public float animSpeedMult;

    public void BeginState(Mob c, Dictionary<string, object> args = null)
    {
        c.mobAnimator.ResetAllAnimatorTriggers();
        c.mobAnimator.SetTrigger(ANIMATION_TRIGGER_MOVE);
        stateInterval = Random.Range(minInterval, maxInterval);

        float wanderX = Random.Range(-1.0F, 1.0F);
        float wanderZ = Random.Range(-1.0F, 1.0F);
        wanderTargetDirection = new Vector3(wanderX, 0.0F, wanderZ).normalized;
        wanderDirection = c.mobDirectionObject.transform.forward;

        wallCheck = MobStatics.GetMobBehaviour<WallCheckMobBehaviour>(c, MobBehaviourId.WallCheck);

        if(wallCheck != null)
            didBeginStateHittingWall = wallCheck.IsWallHit;
    }

    public void FixedUpdateState(Mob c) 
    {
        if(c.mobRigidBody.velocity.magnitude < wanderMaxVel)
            c.mobRigidBody.AddForce(wanderDirection * wanderForceMult, ForceMode.VelocityChange);
    }

    public void UpdateState(Mob c)
    {
        wanderDirection = Vector3.RotateTowards(wanderDirection, wanderTargetDirection, Time.deltaTime, 0.0F);

        if (c.StateTimer > stateInterval)
        {
            c.ChangeToRandomState(nextStateIds);
            return;
        }

        if(wallCheck != null
            && wallCheck.IsWallHit 
            && !didBeginStateHittingWall)
        {
            if(wallCheck.HitHorizontalAngle < WANDER_TURN_AROUND_ANGLE)
            {
                c.ChangeToRandomState(onHitWallStateIds);
                return;
            }
            else
            {
                wanderTargetDirection = Vector3.ProjectOnPlane(wanderTargetDirection, wallCheck.HitInfo.normal);
                wanderDirection = wanderTargetDirection;
            }
        }

        // Renderer.
        MobStatics.UpdateInternalDirection(c, wanderDirection);
        MobStatics.UpdateRendererDirection(c, wanderDirection, ANIMATION_TURNING_SPEED_MULT_SLOW);

        // Animator.
        if(doUseVelAnimSpeedMult)
            c.mobAnimator.SetFloat
                ( ANIMATION_TRIGGER_SPEED_MULTIPLIER
                , c.mobRigidBody.velocity.magnitude);
        else
            c.mobAnimator.SetFloat
                (ANIMATION_TRIGGER_SPEED_MULTIPLIER
                , animSpeedMult);
    }

    public void EndState(Mob c) { }
}
