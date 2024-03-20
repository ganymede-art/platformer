using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class DoubleJumpPlayerState : MonoBehaviour, IState<Player,PlayerStateId>
{
    public PlayerStateId StateId => PlayerStateId.DoubleJump;

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger
            (Random.Range(0, 2) == 0
            ? ANIMATION_TRIGGER_DOUBLE_JUMP_UP
            : ANIMATION_TRIGGER_DOUBLE_JUMP_UP_ALTERNATE);

        c.playerRigidBody.velocity = new Vector3
                (c.playerRigidBody.velocity.x, 0, c.playerRigidBody.velocity.z);
        c.playerRigidBody.AddForce(Vector3.up * JUMP_FORCE_MULT, ForceMode.VelocityChange);

        c.doubleJumpSound.PlayPitchedOneShot
            (c.doubleJumpSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);

        c.doubleJumpBeginFx.Play();
    }

    public void EndState(Player c) { }

    public void FixedUpdateState(Player c)
    {
        var direction = PlayerStatics.GetFlatDirectionForMovement(c);
        var force = PlayerStatics.GetForceForMovement(c, direction);
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, STATIC_FRICTION);
        PlayerStatics.FixedUpdateMovement(c, direction, force);
        PlayerStatics.FixedUpdateLimitVelocityTwoAxis(c, DEFAULT_MAX_SPEED);
    }

    public void UpdateState(Player c)
    {
        if (c.StateTimer > JUMP_MIN_INTERVAL
            && c.GroundCheck.IsCheckSphereHit)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if (c.StateTimer > JUMP_MAX_INTERVAL)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if (!InputHighLogic.G.WasWestPressed
            && InputHighLogic.G.IsWestPressed
            && InputHighLogic.G.IsInputActive
            && PlayerHighLogic.G.CanLunge)
        {
            c.ChangeState(PlayerStateId.Lunge);
            return;
        }
    }
}
