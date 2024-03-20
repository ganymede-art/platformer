using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class AttackUnderwaterPlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    public PlayerStateId StateId => PlayerStateId.AttackUnderwater;

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.Gravity.IsGravityEnabled = false;

        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_ATTACK_UNDERWATER);

        c.playerRigidBody.velocity = Vector3.zero;
        c.playerRigidBody.AddForce
            ( c.playerDirectionObject.transform.forward * ATTACK_UNDERWATER_FORE_FORCE_MULT
            , ForceMode.VelocityChange);

        c.attackUnderwaterSound.PlayPitchedOneShot
            (c.attackUnderwaterSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);

        c.attackUnderwaterBeginFx.Play();

        c.attackUnderwaterHitbox.gameObject.SetActive(true);
    }

    public void FixedUpdateState(Player c)
    {
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, STATIC_FRICTION);
        PlayerStatics.FixedUpdateLimitVelocityThreeAxis
            ( c
            , WATER_MAX_SPEED
            , WD_SINK_VEL_LIMIT
            , WD_RISE_VEL_LIMIT);

        // Buoyancy.
        PlayerStatics.FixedUpdateWeakBuoyancy(c);
    }

    public void UpdateState(Player c)
    {
        // State.
        if(!c.Water.IsPartialSubmerged)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if(c.StateTimer >= ATTACK_UNDERWATER_INTERVAL)
        {
            c.ChangeState(PlayerStateId.WaterDefault);
            return;
        }

        // Surface clamp.
        PlayerStatics.UpdateBuoyancy(c);
    }

    public void EndState(Player c)
    {
        c.Gravity.IsGravityEnabled = true;

        c.attackUnderwaterHitbox.gameObject.SetActive(false);
    }
}
