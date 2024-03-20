using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public class HurtMobState : MonoBehaviour, IState<Mob,MobStateId>
{
    // Private fields.
    private GameObject hitboxObject;
    private HitboxData hitboxData;
    private bool isRising;

    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // public fields.
    [Header("State Attributes")]
    public MobStateIdConstant stateId;
    public MobStateIdConstant[] nextStateIds;
    public float interval;

    [Header("Hurt Attributes")]
    public float forceMult;

    public void BeginState(Mob c, Dictionary<string, object> args = null)
    {
        c.mobAnimator.ResetAllAnimatorTriggers();
        c.mobAnimator.SetTrigger(ANIMATION_TRIGGER_HURT_UP);

        c.mobRigidBody.velocity = Vector3.zero;
        c.mobRigidBody.AddForce(Vector3.up * forceMult, ForceMode.VelocityChange);

        if (args != null)
        {
            hitboxObject = args[STATE_ARG_HITBOX_OBJECT] as GameObject;
            hitboxData = args[STATE_ARG_HITBOX_DATA] as HitboxData;

            // If being damaged by the player, use them as the reference point.
            Vector3 hitboxPosition
                = hitboxData.damageType.DamageType == DamageType.Player ? ActiveSceneHighLogic.G.CachedPlayerObject.transform.position
                : hitboxObject.transform.position;

            var awayDirection = (c.transform.position - hitboxPosition).normalized;
            awayDirection.y = 0.0F;

            MobStatics.UpdateInternalDirection(c, -awayDirection);
            c.mobRigidBody.AddForce(awayDirection * forceMult * hitboxData.damageForceMult, ForceMode.VelocityChange);
        }

        isRising = true;

        for (int i = 0; i < c.passiveHitboxes.Length; i++)
            c.passiveHitboxes[i].gameObject.SetActive(false);

        c.mobCollider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        c.mobCollider.material.dynamicFriction = HURT_FRICTION;
        c.mobCollider.material.staticFriction = HURT_FRICTION;
    }

    public void FixedUpdateState(Mob c) { }

    public void UpdateState(Mob c)
    {
        if (c.StateTimer > interval)
        {
            c.ChangeToRandomState(nextStateIds);
            return;
        }

        if(c.mobRigidBody.velocity.y < HURT_MIN_RISING_VEL
            && c.StateTimer > HURT_MIN_RISING_INTERVAL
            && isRising)
        {
            isRising = false;
            c.mobAnimator.ResetAllAnimatorTriggers();
            c.mobAnimator.SetTrigger(ANIMATION_TRIGGER_HURT_DOWN);
        }
    }

    public void EndState(Mob c)
    {
        for (int i = 0; i < c.passiveHitboxes.Length; i++)
            c.passiveHitboxes[i].gameObject.SetActive(true);

        c.RestoreCachedFriction();
    }
}
