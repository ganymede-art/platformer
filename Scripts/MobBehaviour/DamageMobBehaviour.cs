using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Constants;
using static MobConstants;


public class DamageMobBehaviour : MonoBehaviour, IBehaviour<Mob, MobBehaviourId>
{
    // Private fields.
    private IRemoteTrigger remoteTrigger;
    private bool isDamaged;
    private float damageTimer;
    private Dictionary<string, object> damageArgs;

    // Public properties.
    public MobBehaviourId BehaviourId => MobBehaviourId.Damage;

    // Public fields.
    [Header("Behaviour Attributes")]
    public Mob controller;
    [Space]
    public GameObject remoteTriggerObject;
    [Space]
    public MobStateIdConstant[] immuneStateIds;
    public MobStateIdConstant[] vulnerableStateIds;
    public MobStateIdConstant onImmuneStateId;
    public MobStateIdConstant onNotVulnerableStateId;
    public MobStateIdConstant onDamageStateId;
    public MobStateIdConstant onDieStateId;
    public float damageInterval;
    

    private void Awake()
    {
        remoteTrigger = remoteTriggerObject.GetComponent<IRemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEnter;
        remoteTrigger.RemoteTriggerStayed += OnRemoteTriggerStay;
        damageArgs = new Dictionary<string, object>();
    }

    public void BeginBehaviour(Mob c, Dictionary<string, object> args = null) { }
    public void FixedUpdateBehaviour(Mob c) { }
    public void UpdateBehaviour(Mob c)
    {
        if (!isDamaged)
            return;

        damageTimer += Time.deltaTime;

        if (damageTimer > damageInterval)
        {
            isDamaged = false;
            damageTimer = 0.0F;

            if (c.damageActor != null)
                c.damageActor.EndDamage();
        }
    }
    public void EndBehaviours(Mob controller) { }

    public void OnRemoteTriggerEnter(object sender, RemoteTriggerArgs args)
    {
        OnDamage(args.remoteTriggerObject, args.other);
    }

    public void OnRemoteTriggerStay(object sender, RemoteTriggerArgs args)
    {
        OnDamage(args.remoteTriggerObject, args.other);
    }

    public void OnDamage(GameObject remoteTriggerObject, Collider other)
    {
        if (isDamaged)
            return;

        if (!other.isTrigger)
            return;

        if (other.gameObject.layer != LAYER_HITBOX)
            return;

        var hitboxData = ActiveSceneHighLogic.G.HitboxDatas.GetValueOrDefault(other.gameObject);

        if (hitboxData == null)
            return;

        if (hitboxData.damageType.DamageType == DamageType.None
            || hitboxData.damageType.DamageType == DamageType.Mob
            || hitboxData.damageType.DamageType == DamageType.MobIndirect
            || hitboxData.damageType.DamageType == DamageType.MobPassive)
            return;

        isDamaged = true;
        damageArgs[STATE_ARG_HITBOX_OBJECT] = other.gameObject;
        damageArgs[STATE_ARG_HITBOX_DATA] = hitboxData;

        // Check if invulnerable, and move to no damage state if so.
        for (int i = 0; i < immuneStateIds.Length; i++)
            if (controller.ActiveState == immuneStateIds[i].mobStateId)
            {
                if (onImmuneStateId != null)
                    controller.ChangeState(onImmuneStateId.mobStateId, damageArgs);
                return;
            }
                

        if(vulnerableStateIds != null && vulnerableStateIds.Length > 0)
        {
            bool isActiveStateVulnerable = false;
            for (int i = 0; i < vulnerableStateIds.Length; i++)
            {
                if (controller.ActiveState == vulnerableStateIds[i].mobStateId)
                {
                    isActiveStateVulnerable = true;
                    break;
                }
            }

            if(!isActiveStateVulnerable)
            {
                if (onNotVulnerableStateId != null)
                    controller.ChangeState(onNotVulnerableStateId.mobStateId, damageArgs);
                return;
            }
        }
        
        // Set damaged effect.
        if (controller.damageActor != null)
            controller.damageActor.BeginDamage();

        // Decrement health.
        controller.health -= hitboxData.damageAmount;

        // Change to relevant state.
        if (controller.health > 0)
            controller.ChangeState(onDamageStateId.mobStateId, damageArgs);
        else
            controller.ChangeState(onDieStateId.mobStateId, damageArgs);
    }
}
