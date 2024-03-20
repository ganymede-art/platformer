using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class WaterDefaultPlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    // Public properties.
    public PlayerStateId StateId => PlayerStateId.WaterDefault;

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.Gravity.IsGravityEnabled = false;
    }

    public void FixedUpdateState(Player c)
    {
        var direction = PlayerStatics.GetFlatDirectionForMovement(c);
        var force = PlayerStatics.GetForceForMovement(c, direction);
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, STATIC_FRICTION);
        PlayerStatics.FixedUpdateMovement(c, direction, force);
        PlayerStatics.FixedUpdateLimitVelocityThreeAxis
            ( c
            , WATER_MAX_SPEED
            , WD_SINK_VEL_LIMIT
            , WD_RISE_VEL_LIMIT);

        // Buoyancy.
        PlayerStatics.FixedUpdateBuoyancy(c);
    }

    public void UpdateState(Player c)
    {
        // State.
        if(!c.Water.IsPartialSubmerged)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if (!InputHighLogic.G.WasNorthPressed
            && InputHighLogic.G.IsNorthPressed
            && InputHighLogic.G.IsInputActive)
        {
            c.Interact.Interact(c);
        }

        if (!InputHighLogic.G.WasSouthPressed
            && InputHighLogic.G.IsSouthPressed
            && InputHighLogic.G.IsInputActive
            && !c.Water.IsFullSubmerged)
        {
            c.ChangeState(PlayerStateId.Jump);
            return;
        }

        if(!InputHighLogic.G.WasEastPressed
            && InputHighLogic.G.IsEastPressed
            && InputHighLogic.G.IsInputActive
            && PlayerHighLogic.G.CanDiveUnderwater)
        {
            c.ChangeState(PlayerStateId.DiveUnderwater);
            return;
        }

        if (!InputHighLogic.G.WasWestPressed
            && InputHighLogic.G.IsWestPressed
            && InputHighLogic.G.IsInputActive
            && PlayerHighLogic.G.CanAttackUnderwater)
        {
            c.ChangeState(PlayerStateId.AttackUnderwater);
            return;
        }

        // Surface clamp.
        PlayerStatics.UpdateBuoyancy(c);

        // Animator.
        if (c.GroundCheck.IsCheckSphereGrounded)
        {
            if (InputHighLogic.G.Move3d.magnitude > 0.01F)
            {
                c.playerAnimator.ResetAllAnimatorTriggers();
                c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_MOVE);
                c.playerAnimator.SetFloat(ANIMATION_TRIGGER_SPEED_MULTIPLIER, c.playerRigidBody.velocity.magnitude);
            }
            else
            {
                c.playerAnimator.ResetAllAnimatorTriggers();
                c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_IDLE);
            }
        }
        else
        {
            c.playerAnimator.ResetAllAnimatorTriggers();
            c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_SWIM);
            c.playerAnimator.SetFloat(ANIMATION_TRIGGER_SPEED_MULTIPLIER, c.playerRigidBody.velocity.magnitude);
        }

        var direction = PlayerStatics.GetFlatDirectionForMovement(c);
        if (InputHighLogic.G.IsMove3dPressed)
            PlayerStatics.UpdateInternalDirection(c, direction);
        PlayerStatics.UpdateRendererDirection(c, c.playerDirectionObject.transform.forward);
    }

    public void EndState(Player c)
    {
        c.Gravity.IsGravityEnabled = true;
    }
}
