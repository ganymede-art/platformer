using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Constants;
using static PlayerConstants;

public class CrouchPlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    public PlayerStateId StateId => PlayerStateId.Crouch;

    public void BeginState(Player c, Dictionary<string, object> args = null) 
    {
        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_CROUCH);
    }
    public void EndState(Player c) { }

    public void FixedUpdateState(Player c)
    {
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, STATIC_FRICTION);
        PlayerStatics.FixedUpdateLimitVelocityTwoAxis(c, DEFAULT_MAX_SPEED);
    }

    public void UpdateState(Player c)
    {
        // State.
        if(!c.GroundCheck.IsCheckSphereGrounded)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if(c.Water.IsPartialSubmerged)
        {
            c.ChangeState(PlayerStateId.WaterDefault);
            return;
        }

        if (!InputHighLogic.G.WasNorthPressed
            && InputHighLogic.G.IsNorthPressed
            && InputHighLogic.G.IsInputActive
            && c.GroundCheck.IsCheckSphereGrounded)
        {
            c.ChangeState(PlayerStateId.UseKeyItem);
            return;
        }

        if (!InputHighLogic.G.WasSouthPressed
            && InputHighLogic.G.IsSouthPressed
            && InputHighLogic.G.IsInputActive
            && c.GroundCheck.IsCheckSphereGrounded
            && PlayerHighLogic.G.CanHighJump)
        {
            c.ChangeState(PlayerStateId.HighJump);
            return;
        }

        if (!InputHighLogic.G.WasWestPressed
            && InputHighLogic.G.IsWestPressed
            && InputHighLogic.G.IsInputActive
            && c.GroundCheck.IsCheckSphereGrounded
            && PlayerHighLogic.G.CanShoot)
        {
            c.ChangeState(PlayerStateId.Shoot);
            return;
        }

        if(!InputHighLogic.G.IsNearPressed
            && InputHighLogic.G.IsInputActive)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        var direction = PlayerStatics.GetFlatDirectionForMovement(c);
        if(InputHighLogic.G.IsMove3dPressed)
            PlayerStatics.UpdateInternalDirection(c, direction);
        PlayerStatics.UpdateRendererDirection(c, c.playerDirectionObject.transform.forward);
    }
}
