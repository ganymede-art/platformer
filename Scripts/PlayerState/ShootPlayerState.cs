using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class ShootPlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    public PlayerStateId StateId => PlayerStateId.Shoot;

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger
            (Random.Range(0, 2) == 0
            ? ANIMATION_TRIGGER_ATTACK
            : ANIMATION_TRIGGER_ATTACK_ALTERNATE);

        c.playerRigidBody.velocity = Vector3.zero;
        c.playerRigidBody.AddForce(Vector3.up * ATTACK_UP_FORCE_MULT, ForceMode.VelocityChange);

        c.attackSound.PlayPitchedOneShot
            (c.attackSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);

        Instantiate
            ( c.projectilePrefab
            , c.playerDirectionObject.transform.position
            , c.playerDirectionObject.transform.rotation);
    }

    public void FixedUpdateState(Player c)
    {
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, STATIC_FRICTION);
        PlayerStatics.FixedUpdateLimitVelocityTwoAxis(c, DEFAULT_MAX_SPEED);
    }

    public void UpdateState(Player c)
    {
        if (c.StateTimer >= ATTACK_MAX_INTERVAL)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }
    }

    public void EndState(Player c)
    {
        
    }
}
