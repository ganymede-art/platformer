using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Constants;
using static PlayerConstants;

public class DefaultPlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    public PlayerStateId StateId => PlayerStateId.Default;

    public void BeginState(Player c, Dictionary<string, object> args = null) { }
    public void EndState(Player c) { }

    public void FixedUpdateState(Player c)
    {
        var direction = PlayerStatics.GetDirectionForMovement(c);
        var force = PlayerStatics.GetForceForMovement(c, direction);
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, STATIC_FRICTION);
        PlayerStatics.FixedUpdateMovement(c, direction, force);
        PlayerStatics.FixedUpdateLimitVelocityTwoAxis(c, DEFAULT_MAX_SPEED);
    }

    public void UpdateState(Player c)
    {
        // State.
        if(c.Water.IsPartialSubmerged)
        {
            c.ChangeState(PlayerStateId.WaterDefault);
            return;
        }

        if(!InputHighLogic.G.WasNorthPressed
            && InputHighLogic.G.IsNorthPressed
            && InputHighLogic.G.IsInputActive
            && c.GroundCheck.IsCheckSphereGrounded)
        {
            c.Interact.Interact(c);
        }

        if (!InputHighLogic.G.WasSouthPressed
            && InputHighLogic.G.IsSouthPressed
            && InputHighLogic.G.IsInputActive
            && c.GroundCheck.IsCheckSphereGrounded)
        {
            c.ChangeState(PlayerStateId.Jump);
            return;
        }

        if (!InputHighLogic.G.WasSouthPressed
            && InputHighLogic.G.IsSouthPressed
            && InputHighLogic.G.IsInputActive
            && PlayerHighLogic.G.CanDoubleJump
            && c.PreviousState == PlayerStateId.Jump
            && !c.GroundCheck.IsCheckSphereGrounded
            && !c.GroundCheck.WasCheckSphereGroundedAfterBegin)
        {
            c.ChangeState(PlayerStateId.DoubleJump);
            return;
        }

        if (!InputHighLogic.G.WasWestPressed
            && InputHighLogic.G.IsWestPressed
            && InputHighLogic.G.IsInputActive
            && PlayerHighLogic.G.CanLunge
            && c.PreviousState == PlayerStateId.Jump
            && !c.GroundCheck.IsCheckSphereGrounded
            && !c.GroundCheck.WasCheckSphereGroundedAfterBegin)
        {
            c.ChangeState(PlayerStateId.Lunge);
            return;
        }

        if (!InputHighLogic.G.WasWestPressed
            && InputHighLogic.G.IsWestPressed
            && InputHighLogic.G.IsInputActive
            && c.GroundCheck.IsCheckSphereGrounded
            && PlayerHighLogic.G.CanAttack)
        {
            c.ChangeState(PlayerStateId.Attack);
            return;
        }

        if(InputHighLogic.G.IsNearPressed
            && InputHighLogic.G.IsInputActive)
        {
            c.ChangeState(PlayerStateId.Crouch);
            return;
        }

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
            c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_JUMP_DOWN);
        }

        var direction = PlayerStatics.GetFlatDirectionForMovement(c);
        if(InputHighLogic.G.IsMove3dPressed)
            PlayerStatics.UpdateInternalDirection(c, direction);
        PlayerStatics.UpdateRendererDirection(c, c.playerDirectionObject.transform.forward);
    }
}
