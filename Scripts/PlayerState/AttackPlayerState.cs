using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class AttackPlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    // Private fields.
    private IRemoteTrigger remoteTrigger;

    // Public properties.
    public PlayerStateId StateId => PlayerStateId.Attack;

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
        c.playerAnimator.SetTrigger
            ( Random.Range(0, 2) == 0 
            ? ANIMATION_TRIGGER_ATTACK 
            : ANIMATION_TRIGGER_ATTACK_ALTERNATE);

        c.playerRigidBody.velocity = Vector3.zero;
        c.playerRigidBody.AddForce(Vector3.up * ATTACK_UP_FORCE_MULT, ForceMode.VelocityChange);
        c.playerRigidBody.AddForce(c.playerDirectionObject.transform.forward * ATTACK_FORE_FORCE_MULT, ForceMode.VelocityChange);

        c.attackSound.PlayPitchedOneShot
            (c.attackSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);

        c.attackHitbox.gameObject.SetActive(true);
    }

    public void FixedUpdateState(Player c)
    {
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, STATIC_FRICTION);
        PlayerStatics.FixedUpdateLimitVelocityTwoAxis(c, DEFAULT_MAX_SPEED);
    }

    public void UpdateState(Player c)
    {
        if(c.StateTimer >= ATTACK_MAX_INTERVAL)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }
    }

    public void EndState(Player c)
    {
        c.attackHitbox.gameObject.SetActive(false);
    }

    public void OnRemoteTriggerEnter(object sender, RemoteTriggerArgs args)
    {
        if (controller.ActiveState != StateId)
            return;

        if (remoteTriggerObject.name != TRANSFORM_NAME_PLAYER_ATTACK_HITBOX)
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
