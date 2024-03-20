
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class DamagePlayerBehaviour : MonoBehaviour, IBehaviour<Player, PlayerBehaviourId>
{
    // Private fields.
    private IRemoteTrigger remoteTrigger;
    private bool isDamaged;
    private float damageTimer;
    private Dictionary<string, object> damageArgs;

    // Public properties.
    public PlayerBehaviourId BehaviourId => PlayerBehaviourId.Damage;

    // Public fields.
    public Player controller;
    public GameObject remoteTriggerObject;

    private void Awake()
    {
        remoteTrigger = remoteTriggerObject.GetComponent<IRemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEnter;
        remoteTrigger.RemoteTriggerStayed += OnRemoteTriggerStay;

        damageArgs = new Dictionary<string, object>();
    }

    private void OnDestroy()
    {
        if (remoteTrigger != null)
        {
            remoteTrigger.RemoteTriggerEntered -= OnRemoteTriggerEnter;
            remoteTrigger.RemoteTriggerStayed -= OnRemoteTriggerStay;
        }
    }

    public void BeginBehaviour(Player c, Dictionary<string, object> args = null) { }
    public void FixedUpdateBehaviour(Player c) { }

    public void UpdateBehaviour(Player c)
    {
        if (!isDamaged)
            return;

        damageTimer += Time.deltaTime;

        if (damageTimer > DAMAGE_INTERVAL)
        {      
            isDamaged = false;
            damageTimer = 0.0F;
            ActiveSceneHighLogic.G.CachedPlayer.playerDamageActor.EndDamage();
        }
    }

    public void EndBehaviours(Player c) { }

    public void OnRemoteTriggerEnter(object sender, RemoteTriggerArgs args)
    {
        OnDamage(args.remoteTriggerObject, args.other);
    }

    public void OnRemoteTriggerStay(object sender, RemoteTriggerArgs args)
    {
        OnDamage(args.remoteTriggerObject, args.other);
    }

    public void OnSimpleDamage(int damageAmount)
    {
        if (isDamaged)
            return;

        if (controller.ActiveState == PlayerStateId.Die)
            return;

        isDamaged = true;
        ActiveSceneHighLogic.G.CachedPlayer.playerDamageActor.BeginDamage();
        PlayerHighLogic.G.ModifyHealth(-damageAmount);

        ActiveSceneHighLogic.G.CachedPlayer.damageAudioSource.PlayPitchedOneShot
            (ActiveSceneHighLogic.G.CachedPlayer.damageAudioSource.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);

        if(PlayerHighLogic.G.Health <= 0)
        {
            ActiveSceneHighLogic.G.CachedPlayer.ChangeState(PlayerStateId.Die);
        }
    }

    public void OnDamage(GameObject remoteTriggerObject, Collider other)
    {
        if (isDamaged)
            return;

        if (controller.ActiveState == PlayerStateId.Die)
            return;

        if (!other.isTrigger)
            return;

        if (other.gameObject.layer != LAYER_HITBOX)
            return;

        var hitboxData = ActiveSceneHighLogic.G.HitboxDatas.GetValueOrDefault(other.gameObject);

        if (hitboxData == null)
            return;

        if (hitboxData.damageType.DamageType == DamageType.None
            || hitboxData.damageType.DamageType == DamageType.Player
            || hitboxData.damageType.DamageType == DamageType.PlayerIndirect)
            return;

        isDamaged = true;
        ActiveSceneHighLogic.G.CachedPlayer.playerDamageActor.BeginDamage();
        PlayerHighLogic.G.ModifyHealth(-hitboxData.damageAmount);

        ActiveSceneHighLogic.G.CachedPlayer.damageAudioSource.PlayPitchedOneShot
            (ActiveSceneHighLogic.G.CachedPlayer.damageAudioSource.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);

        damageArgs[STATE_ARG_HITBOX_OBJECT] = other.gameObject;
        damageArgs[STATE_ARG_HITBOX_DATA] = hitboxData;

        if (PlayerHighLogic.G.Health > 0)
        {
            if (hitboxData.damageForceMult > 0)
                ActiveSceneHighLogic.G.CachedPlayer.ChangeState(PlayerStateId.Hurt, damageArgs);
        }
        else
        {
            ActiveSceneHighLogic.G.CachedPlayer.ChangeState(PlayerStateId.Die);
        }   
    }
}
