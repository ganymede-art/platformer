using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class LungePlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    // Private fields.
    private IRemoteTrigger remoteTrigger;

    // Public properties.
    public PlayerStateId StateId => PlayerStateId.Lunge;

    // Public fields.
    public Player controller;
    public GameObject remoteTriggerObject;

    private void Awake()
    {
        remoteTrigger = remoteTriggerObject.GetComponent<IRemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEnter;
    }

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_LUNGE_UP);

        var direction = PlayerStatics.GetFlatDirectionForMovement(c);
        if (InputHighLogic.G.IsMove3dPressed)
            PlayerStatics.UpdateInternalDirection(c, direction);

        c.playerRigidBody.velocity = new Vector3
                (c.playerRigidBody.velocity.x, 0, c.playerRigidBody.velocity.z);
        c.playerRigidBody.AddForce
            ( Vector3.up * LUNGE_UP_FORCE_MULT
            , ForceMode.VelocityChange);
        c.playerRigidBody.AddForce
            ( c.playerDirectionObject.transform.forward * LUNGE_FORE_FORCE_MULT
            , ForceMode.VelocityChange);

        c.lungeSound.PlayPitchedOneShot
            (c.lungeSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);

        c.lungeHitbox.gameObject.SetActive(true);
    }

    public void FixedUpdateState(Player c)
    {
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, DYNAMIC_FRICTION);
        PlayerStatics.FixedUpdateLimitVelocityTwoAxis(c, LUNGE_MAX_SPEED);
    }

    public void UpdateState(Player c)
    {
        // States.
        if (c.StateTimer > LUNGE_MIN_INTERVAL
            && c.playerRigidBody.velocity.magnitude < LUNGE_MIN_VEL
            && c.GroundCheck.IsCheckSphereHit)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if(c.StateTimer > LUNGE_MIN_INTERVAL
            && !InputHighLogic.G.IsWestPressed)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if (c.StateTimer > LUNGE_MAX_INTERVAL)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if (!InputHighLogic.G.WasSouthPressed
            && InputHighLogic.G.IsSouthPressed
            && InputHighLogic.G.IsInputActive
            && PlayerHighLogic.G.CanDoubleJump
            && c.GroundCheck.IsCheckSphereGrounded)
        {
            c.ChangeState(PlayerStateId.DoubleJump);
            return;
        }

        // Animator.
        if (c.GroundCheck.IsCheckSphereHit)
        {
            c.playerAnimator.ResetAllAnimatorTriggers();
            c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_LUNGE_DOWN);
        }

        // Renderer.
        PlayerStatics.UpdateRendererDirection(c, c.playerDirectionObject.transform.forward);
    }

    public void EndState(Player c)
    {
        c.lungeHitbox.gameObject.SetActive(false);
    }

    public void OnRemoteTriggerEnter(object sender, RemoteTriggerArgs args)
    {
        if (controller.ActiveState != StateId)
            return;

        if (remoteTriggerObject.name != TRANSFORM_NAME_PLAYER_LUNGE_HITBOX)
            return;

        if (args.other.gameObject.layer != LAYER_HITBOX)
            return;

        HitboxData hitboxData = null;
        ActiveSceneHighLogic.G.HitboxDatas.TryGetValue(args.other.gameObject, out hitboxData);

        if (hitboxData == null)
            return;

        if (hitboxData.damageType.DamageType == DamageType.None
            || hitboxData.damageType.DamageType == DamageType.MobPassive)
            ActiveSceneHighLogic.G.CachedPlayer.ChangeState(PlayerStateId.AttackRecoil);
    }
}