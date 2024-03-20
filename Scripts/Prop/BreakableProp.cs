using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class BreakableProp : MonoBehaviour
{
    // Private fields.
    private IRemoteTrigger remoteTrigger;

    // Public fields.
    [Header("Breakable Attributes")]
    public bool canPlayerBreak;
    public bool canMobBreak;
    public bool canStaticBreak;
    [Space]
    public bool doRepel;
    [Space]
    public GameObject remoteTriggerObject;
    public GameObject onDiePrefab;
    public Vector3 onDiePrefabSpawnOffset;
    public GameObject fxPrefab;
    public Vector3 fxPrefabSpawnOffset;

    private void Awake()
    {
        remoteTrigger = remoteTriggerObject.GetComponent<IRemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEnter;
    }

    private void Break()
    {
        if (onDiePrefab != null)
            Instantiate(onDiePrefab, transform.position + onDiePrefabSpawnOffset, transform.rotation);

        if(fxPrefab != null)
            
            Instantiate(fxPrefab, transform.position + fxPrefabSpawnOffset, transform.rotation);
        Destroy(gameObject);
    }

    public void OnRemoteTriggerEnter(object sender, RemoteTriggerArgs args)
    {
        if (args.other.gameObject.layer != LAYER_HITBOX)
            return;

        var hitboxData = ActiveSceneHighLogic.G.HitboxDatas[args.other.gameObject];

        if (hitboxData.damageType.DamageType == DamageType.Player
            && canPlayerBreak)
        {
            Break();
        }
        else if (hitboxData.damageType.DamageType == DamageType.PlayerIndirect
            && canPlayerBreak)
        {
            Break();
        }
        else if (hitboxData.damageType.DamageType == DamageType.Mob)
        {
            Break();
        }
        else if (hitboxData.damageType.DamageType == DamageType.MobIndirect)
        {
            Break();
        }
    }
}
