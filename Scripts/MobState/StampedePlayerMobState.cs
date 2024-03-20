using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public class StampedePlayerMobState : MonoBehaviour, IState<Mob,MobStateId>
{
    // Private fields.
    private Vector3 stampedeDirection;
    private WallCheckMobBehaviour wallCheck;

    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // Public fields.
    [Header("State Attributes")]
    public MobStateIdConstant stateId;
    public MobStateIdConstant[] onIntervalStateIds;
    public MobStateIdConstant[] onHitWallStateIds;
    public MobStateIdConstant[] onHitBoundaryStateIds;
    [Space]
    public float interval;

    [Header("Wander Attributes")]
    public float stampedeForceMult;
    public float stampedeMaxVel;

    [Header("Stampede Player Attributes")]
    public Collider beginHitbox;

    public void BeginState(Mob c, Dictionary<string, object> args = null)
    {
        // Init private fields.
        stampedeDirection
            = (ActiveSceneHighLogic.G.CachedPlayerObject.transform.position
            - c.transform.position).normalized;
        stampedeDirection.y = 0.0F;
        wallCheck = MobStatics.GetMobBehaviour<WallCheckMobBehaviour>(c, MobBehaviourId.WallCheck);

        // Init controller.
        c.mobAnimator.ResetAllAnimatorTriggers();
        c.mobAnimator.SetTrigger(ANIMATION_TRIGGER_CHARGE);

        if (beginHitbox != null)
            beginHitbox.gameObject.SetActive(true);
    }

    public void FixedUpdateState(Mob c) 
    {
        if (c.mobRigidBody.velocity.magnitude < stampedeMaxVel)
            c.mobRigidBody.AddForce(stampedeDirection * stampedeForceMult, ForceMode.VelocityChange);
    }

    public void UpdateState(Mob c)
    {
        if (c.StateTimer > interval)
        {
            c.ChangeToRandomState(onIntervalStateIds);
            return;
        }

        if(wallCheck != null
            && wallCheck.IsWallHit)
        {
            if (wallCheck.HitInfo.collider.gameObject.layer == SCENE_BOUNDARY
                || wallCheck.HitInfo.collider.gameObject.layer == SCENE_BOUNDARY_IGNORE_CAMERA
                || wallCheck.HitInfo.collider.gameObject.layer == LAYER_MOB_ONLY)
                c.ChangeToRandomState(onHitBoundaryStateIds);
            else
                c.ChangeToRandomState(onHitWallStateIds);
            return;
        }

        MobStatics.UpdateInternalDirection(c, stampedeDirection);
        MobStatics.UpdateRendererDirection(c, stampedeDirection);

        // Animator.
        c.mobAnimator.SetFloat
            (ANIMATION_TRIGGER_SPEED_MULTIPLIER
            , c.mobRigidBody.velocity.magnitude * ANIMATION_MAGNITUDE_SPEED_MULT);
    }

    public void EndState(Mob c) 
    {
        if (beginHitbox != null)
            beginHitbox.gameObject.SetActive(false);
    }
}
