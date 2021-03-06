using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assets.Script.GameConstants;
using UnityEngine;
using Assets.Script;


public class MobBehaviourDamage : MonoBehaviour, IMobBehaviour
{
    // damage constants.

    public const float DAMAGE_INTERVAL = 1.5F;

    private MobController mobController;
    private ActorDamageEffectController damageEffectController;

    [NonSerialized] public bool isDamaged;
    [NonSerialized] public float damageTimer;
    [NonSerialized] public float damageInterval;

    [Header("MobController Attributes")]
    public GameObject mobControllerObject;

    [Header("State Attributes")]
    public bool canAlwaysBeDamaged;
    public string[] canBeDamagedStates;
    public string[] cannotBeDamagedStates;
    public string damagedState;
    public string deathState;

    [Header("Behaviour Attributes")]
    public bool canBeDamagedByPlayer;
    public bool canBeDamagedByNpc;
    public bool canBeDamagedByMob;
    public bool canBeDamagedByStatic;

    [Header("Damage Attributes")]
    public int health;
    public GameObject damageEffectControllerObject;


    public string GetBehaviourType()
    {
        return MOB_BEHAVIOUR_DAMAGE;
    }

    private void Start()
    {
        // get the mob controller.
        mobController = mobControllerObject.GetComponent<MobController>();

        // get the damage effect controller.
        damageEffectController = damageEffectControllerObject.GetComponent<ActorDamageEffectController>();

        isDamaged = false;
        damageTimer = 0.0F;
        damageInterval = DAMAGE_INTERVAL; 
    }

    private void Update()
    {
        // check if in game state.
        if (GameMasterController.Global.gameState != GAME_STATE_GAME)
            return;

        if(isDamaged)
        {
            damageTimer += Time.deltaTime;
            if(damageTimer > damageInterval)
            {
                damageEffectController.UnsetDamageEffect();
                isDamaged = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(canBeDamagedByPlayer)
        {
            if(other.tag == TAG_PLAYER_DAMAGE_SOURCE
            || other.tag == TAG_PLAYER_INDIRECT_DAMAGE_SOURCE)
            {
                HandleDamage(other.gameObject);
            }
        }
    }

    private void HandleDamage(GameObject sourceObject)
    {
        if (isDamaged)
            return; 

        var damageData = sourceObject.GetComponent<DamageDataController>()?.damageData;

        if (damageData == null)
            damageData = GameDefaultsController.Global.defaultDamageData;

        health -= damageData.damageAmount;

        damageEffectController.SetDamageEffect();

        isDamaged = true;
        damageTimer = 0.0F;

        if (health > 0)
            mobController.ChangeState(damagedState, sourceObject);
        else
            mobController.ChangeState(deathState);

        // if the damage came from the player, add a repel force.
        if (sourceObject.tag == TAG_PLAYER_DAMAGE_SOURCE)
            GameMasterController.GlobalPlayerController.SimpleRepel(gameObject, 5.0F);
    }
}

