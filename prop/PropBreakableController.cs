using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.Serialization;
using static Assets.Script.prop.PropConstants;

public class PropBreakableController : MonoBehaviour
{
    private const float BASE_VOLUME = 1F;

    private GameMasterController master;

    private GameObject destroyFxObject;
    private AudioSource destroyAudioSource;
    private float pickupAudioPitch;
    private float pickupAudioVolume;

    [Header("Prop Attributes")]
    public int health;

    [Header("Destroy Attributes")]
    public bool isDestroyOneShot;
    public string destroyOneShotVarName;

    public GameObject destroyFxPrefab;
    public Transform destroyFxOrigin;

    public float destroyRepelForceMultiplier;

    [Header("Damage Attributes")]
    public GameObject damageFxPrefab;
    public Transform damageFxOrigin;

    public float damageRepelForceMultiplier;

    [Header("Random Drop Attributes")]
    public RandomDropData[] randomDropDatas;
    public Vector3 randomDropSpawnOffset;

    [Header("Sound Effects")]
    [FormerlySerializedAs("sfxDestroy")]
    public AudioClip destroySound;
    [FormerlySerializedAs("sfxDamage")]
    public AudioClip damageSound;

    void Start()
    {
        master = GameMasterController.Global;

        // destroy if already saved as destroyed.

        if(isDestroyOneShot)
        {
            if (master.dataController.GetGameVarBool(destroyOneShotVarName))
                Destroy(this.gameObject);
        }

        // otherwise set up.

        if (destroyFxOrigin == null)
            destroyFxOrigin = this.transform;

        if (destroyRepelForceMultiplier < PROP_REPEL_FORCE_MIN)
            destroyRepelForceMultiplier = PROP_REPEL_FORCE_MIN;

        pickupAudioPitch = Random.Range(0.95f, 1.05f);
        pickupAudioVolume = BASE_VOLUME * master.audioController.volumeObject;

        // validation.

        if (health < PROP_HEALTH_MIN)
            health = PROP_HEALTH_MIN;

        if (damageFxPrefab == null)
            damageFxPrefab = destroyFxPrefab;

        if (damageFxOrigin == null)
            damageFxOrigin = destroyFxOrigin;

        if (damageRepelForceMultiplier < PROP_REPEL_FORCE_MIN)
            damageRepelForceMultiplier = PROP_REPEL_FORCE_MIN;

        if (damageSound == null)
            damageSound = destroySound;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameConstants.TAG_PLAYER_DAMAGE_SOURCE
            || other.transform.root.tag == GameConstants.TAG_PLAYER_INDIRECT_DAMAGE_SOURCE)
        {
            HandleDamageObject(other);
        }
    }

    private void HandleDamageObject(Collider other)
    {
        Debug.Log("[PropBreakableController] Hit.");
        // handle damage, destroy if out of health.

        var damageType = other.gameObject.GetComponent<DamageDataController>()?.damageData;
        if (damageType == null)
            damageType = GameDefaultsController.Global.defaultDamageData;

        health -= damageType.damageAmount;

        if (health <= 0)
        {
            DestroyProp(other);
        }
        else
        {
            DamageProp(other);
        }
    }

    public void DestroyProp(Collider other)
    {
        if (isDestroyOneShot)
            master.dataController.UpdateGameVar(destroyOneShotVarName, true);

        // create destroy fx.

        destroyFxObject = Instantiate(
            destroyFxPrefab,
            destroyFxOrigin.position,
            destroyFxOrigin.rotation);

        destroyAudioSource = destroyFxObject.AddComponent<AudioSource>();
        destroyAudioSource.clip = destroySound;
        destroyAudioSource.pitch = pickupAudioPitch;
        destroyAudioSource.volume = pickupAudioVolume;
        destroyAudioSource.Play();

        // if there are any random drops, spawn them.
        
        if(randomDropDatas != null && randomDropDatas.Length > 0)
        {
            foreach (var randomDropData in randomDropDatas)
            {
                float dropChance = Random.Range(0.0F, 1.0F);
                if (dropChance <= randomDropData.randomDropChance)
                    Instantiate(randomDropData.randomDropPrefab
                        , this.transform.position + randomDropSpawnOffset
                        , this.transform.rotation);
            }
        }

        // destroy self and particle fx.

        Destroy(this.gameObject);
        Destroy(destroyFxObject, 5);

        // if the collidng object is the player itself, repel them.

        if (other.transform.root.gameObject.name == GameConstants.NAME_PLAYER)
            GameMasterController.GlobalPlayerController.SimpleRepel(this.gameObject, destroyRepelForceMultiplier);

    }

    public void DamageProp(Collider other)
    {
        // create damage fx.

        destroyFxObject = Instantiate(
            damageFxPrefab,
            damageFxOrigin.position,
            damageFxOrigin.rotation);

        destroyAudioSource = destroyFxObject.AddComponent<AudioSource>();
        destroyAudioSource.clip = damageSound;
        destroyAudioSource.pitch = pickupAudioPitch;
        destroyAudioSource.volume = pickupAudioVolume;
        destroyAudioSource.Play();

        Destroy(destroyFxObject, 5);

        // if the colliding object is the player itself, repel them.

        if (other.transform.root.gameObject.name == GameConstants.NAME_PLAYER)
            GameMasterController.GlobalPlayerController.SimpleRepel(this.gameObject, damageRepelForceMultiplier);

    }
}
