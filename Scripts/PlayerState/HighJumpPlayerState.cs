using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class HighJumpPlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    public PlayerStateId StateId => PlayerStateId.HighJump;

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_HIGH_JUMP_UP);

        c.playerRigidBody.velocity = new Vector3
                (c.playerRigidBody.velocity.x, 0, c.playerRigidBody.velocity.z);
        c.playerRigidBody.AddForce(Vector3.up * HIGH_JUMP_FORCE_MULT, ForceMode.VelocityChange);

        c.highJumpSound.PlayPitchedOneShot
            ( c.highJumpSound.clip
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
        if (c.StateTimer > HIGH_JUMP_MIN_INTERVAL
            && c.GroundCheck.IsCheckSphereHit)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if (c.StateTimer > HIGH_JUMP_MAX_INTERVAL)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }
    }
}
