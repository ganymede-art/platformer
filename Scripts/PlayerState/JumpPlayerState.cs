using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class JumpPlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    public PlayerStateId StateId => PlayerStateId.Jump;

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_JUMP_UP);

        c.playerRigidBody.velocity = new Vector3
                (c.playerRigidBody.velocity.x, 0, c.playerRigidBody.velocity.z);
        c.playerRigidBody.AddForce(Vector3.up * JUMP_FORCE_MULT, ForceMode.VelocityChange);

        c.jumpSound.PlayPitchedOneShot
            ( c.jumpSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);
    }

    public void EndState(Player c) { }

    public void FixedUpdateState(Player c)
    {
        var direction = PlayerStatics.GetFlatDirectionForMovement(c);
        var force = PlayerStatics.GetForceForMovement(c, direction);
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, STATIC_FRICTION);
        PlayerStatics.FixedUpdateMovement(c, direction, force);
        if (InputHighLogic.G.IsSouthPressed
            && c.StateTimer < JUMP_PERSIST_MAX_INTERVAL)
            c.playerRigidBody.AddForce(Vector3.up * JUMP_PERSIST_FORCE_MULT, ForceMode.VelocityChange);
        PlayerStatics.FixedUpdateLimitVelocityTwoAxis(c, DEFAULT_MAX_SPEED);
    }

    public void UpdateState(Player c)
    {
        // States.
        if(!InputHighLogic.G.WasSouthPressed
            && InputHighLogic.G.IsSouthPressed
            && InputHighLogic.G.IsInputActive
            && PlayerHighLogic.G.CanDoubleJump)
        {
            c.ChangeState(PlayerStateId.DoubleJump);
            return;
        }

        if(!InputHighLogic.G.WasWestPressed
            && InputHighLogic.G.IsWestPressed
            && InputHighLogic.G.IsInputActive
            && PlayerHighLogic.G.CanLunge)
        {
            c.ChangeState(PlayerStateId.Lunge);
            return;
        }

        if (!InputHighLogic.G.WasEastPressed
            && InputHighLogic.G.IsEastPressed
            && InputHighLogic.G.IsInputActive
            && PlayerHighLogic.G.CanSlam)
        {
            c.ChangeState(PlayerStateId.Slam);
            return;
        }

        if (c.StateTimer > JUMP_MIN_INTERVAL
            && c.GroundCheck.IsCheckSphereHit)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if (c.StateTimer > JUMP_MIN_INTERVAL
            && c.playerRigidBody.velocity.y < 0.0F)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if (c.StateTimer > JUMP_MAX_INTERVAL)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }
    }
}
