using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static Assets.Script.GameConstants;
using System;
using Assets.Script;

public class PlayerBehaviourDamage : MonoBehaviour, IPlayerBehaviour
{
    // damage constants.

    public const float DAMAGE_INTERVAL = 3.0F;

    // audio variables.

    private AudioSource audioSource;

    // private damage variables.

    private ActorDamageEffectController damageEffectController;

    // public damage variables.

    [NonSerialized] public GameObject damageSourceObject = null;
    [NonSerialized] public DamageData damageData = null;

    [NonSerialized] public bool isDamaged = false;
    [NonSerialized] public float damageTimer = 0.0F;
    [NonSerialized] public float damageInterval = DAMAGE_INTERVAL;

    // inspector fields;

    public GameObject damageEffectControllerObject;
    public AudioClip defaultHurtSound;

    private void Start()
    {
        // get or create audio source.

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // get the damage effect controller.

        damageEffectController = damageEffectControllerObject.GetComponent<ActorDamageEffectController>();
    }

    private void Update()
    {
        if (GameMasterController.Global.gameState == GAME_STATE_GAME)
        {
            if (isDamaged)
            {
                damageTimer += Time.deltaTime;

                if (damageTimer >= DAMAGE_INTERVAL)
                    UnsetDamaged();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == TAG_MOB_DAMAGE_SOURCE
            || other.tag == TAG_MOB_INDIRECT_DAMAGE_SOURCE
            || other.tag == TAG_STATIC_DAMAGE_SOURCE
            || other.tag == TAG_STATIC_INDIRECT_DAMAGE_SOURCE)
            HandleDamageObject(other.gameObject,false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == TAG_MOB_DAMAGE_SOURCE
            || other.tag == TAG_MOB_INDIRECT_DAMAGE_SOURCE
            || other.tag == TAG_STATIC_DAMAGE_SOURCE
            || other.tag == TAG_STATIC_INDIRECT_DAMAGE_SOURCE)
            HandleDamageObject(other.gameObject, true);
    }

    private void HandleDamageObject(GameObject damageObject, bool isOnTriggerStay)
    {
        // get the objects damage attributes (or default)
        // the handle moving into the damage state.

        damageSourceObject = damageObject;
        damageData = damageObject.GetComponent<DamageDataController>()?.damageData;
        if (damageData == null)
            damageData = GameDefaultsController.Global.defaultDamageData;

        // move to damage mode if not already in damage mode, or instant damage.
        // instant damage applied only if entering the trigger (not on stay).

        if (!isDamaged || (damageData.isDamageInstant && !isOnTriggerStay))
        {
            SetDamaged(damageData);
        }
    }

    public void SetDamaged(DamageData damageData)
    {
        isDamaged = true;

        damageTimer = 0.0F;
        GameMasterController.GlobalPlayerController.ChangePlayerState(GameConstants.PLAYER_STATE_HURT, damageSourceObject, damageData);

        audioSource.clip = defaultHurtSound;
        audioSource.Play();

        damageEffectController.SetDamageEffect();

        GamePlayerController.Global.ModifyPlayerHealth(damageData.damageAmount * -1);
    }

    public void UnsetDamaged()
    {
        isDamaged = false;

        damageTimer = 0.0F;
        damageEffectController.UnsetDamageEffect();
    }

    public string GetBehaviourType()
    {
        return PLAYER_BEHAVIOUR_DAMAGE;
    }

    public void SimpleDamage(int damageAmount, GameObject damageSource = null, float damageForceMultiplier = 0.0F)
    {
        isDamaged = true;
        damageTimer = 0F;

        audioSource.clip = defaultHurtSound;
        audioSource.Play();

        damageEffectController.SetDamageEffect();

        GamePlayerController.Global.ModifyPlayerHealth(-damageAmount);

        if (damageSource != null && damageForceMultiplier > 0)
        {
            GameMasterController.GlobalPlayerController.rigidBody.velocity 
                = Vector3.zero;

            var damageVector 
                = (this.transform.position - damageSource.transform.position).normalized;

            GameMasterController.GlobalPlayerController.rigidBody.AddForce
                (damageVector * damageForceMultiplier, ForceMode.VelocityChange);
        }
    }
}
