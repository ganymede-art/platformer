using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class DiveUnderwaterPlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    public PlayerStateId StateId => PlayerStateId.DiveUnderwater;

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.Gravity.IsGravityEnabled = false;

        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_DIVE_UNDERWATER_DOWN);

        c.playerRigidBody.velocity = new Vector3
                (c.playerRigidBody.velocity.x, 0, c.playerRigidBody.velocity.z);
        c.playerRigidBody.AddForce(Vector3.down * DIVE_UNDERWATER_FORCE_MULT, ForceMode.VelocityChange);

        c.diveUnderwaterSound.PlayPitchedOneShot
            (c.diveUnderwaterSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);
    }

    public void FixedUpdateState(Player c)
    {
        var direction = PlayerStatics.GetFlatDirectionForMovement(c);
        var force = direction * ACCELERATION_AIR;
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, STATIC_FRICTION);
        PlayerStatics.FixedUpdateMovement(c, direction, force);
        PlayerStatics.FixedUpdateLimitVelocityThreeAxis
            ( c
            , WATER_MAX_SPEED
            , WD_SINK_VEL_LIMIT
            , WD_RISE_VEL_LIMIT);
    }

    public void UpdateState(Player c)
    {
        // State.
        if (!c.Water.IsPartialSubmerged)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if (c.StateTimer > DIVE_UNDERWATER_MIN_INTERVAL
            && !InputHighLogic.G.IsEastPressed)
        {
            c.ChangeState(PlayerStateId.WaterDefault);
            return;
        }

        if(c.StateTimer > DIVE_UNDERWATER_MAX_INTERVAL)
        {
            c.ChangeState(PlayerStateId.WaterDefault);
            return;
        }
    }

    public void EndState(Player c)
    {
        c.Gravity.IsGravityEnabled = true;
    }
}