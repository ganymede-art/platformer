using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class HurtPlayerState : MonoBehaviour, IState<Player,PlayerStateId>
{
    // Public properties.
    public PlayerStateId StateId => PlayerStateId.Hurt;

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_HURT_UP);

        c.playerRigidBody.velocity = new Vector3
                (c.playerRigidBody.velocity.x, 0, c.playerRigidBody.velocity.z);
        c.playerRigidBody.AddForce(Vector3.up * HURT_UP_FORCE_MULT, ForceMode.VelocityChange);

        if(args != null)
        {
            var hitboxData = args[STATE_ARG_HITBOX_DATA] as HitboxData;
            var hitboxObject = args[STATE_ARG_HITBOX_OBJECT] as GameObject;

            var awayDirection = (c.transform.position - hitboxObject.transform.position);
            awayDirection.y = 0.0F;
            awayDirection.Normalize();

            PlayerStatics.UpdateInternalDirection(c, -awayDirection);
            c.playerRigidBody.AddForce(awayDirection * HURT_AWAY_FORCE_MULT, ForceMode.VelocityChange);
        }

        c.hurtSound.PlayPitchedOneShot
            (c.hurtSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);

        c.hurtBeginFx.Play();
    }

    public void EndState(Player c) { }

    public void FixedUpdateState(Player c)
    {
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, DYNAMIC_FRICTION);
        PlayerStatics.FixedUpdateLimitVelocityTwoAxis(c, HURT_MAX_SPEED);
    }

    public void UpdateState(Player c)
    {
        // States.
        if (c.StateTimer > HURT_MIN_INTERVAL
            && c.GroundCheck.IsCheckSphereHit)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if(c.StateTimer > HURT_MAX_INTERVAL)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        // Animator.
        if(c.playerRigidBody.velocity.y < HURT_FALL_TRIGGER_MIN_VELOCITY
            && c.StateTimer > HURT_FALL_TRIGGER_MIN_INTERVAL)
        {
            c.playerAnimator.ResetAllAnimatorTriggers();
            c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_HURT_DOWN);
        }

        // Renderer.
        PlayerStatics.UpdateRendererDirection(c, c.playerDirectionObject.transform.forward);
    }
}
